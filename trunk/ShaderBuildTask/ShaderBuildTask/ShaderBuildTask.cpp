// This is the main DLL file.

#include "stdafx.h"
#include <atlstr.h>

#include "ShaderBuildTask.h"
#include <msclr/marshal.h>

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace System::Text::RegularExpressions;
using namespace msclr::interop; 


namespace ShaderBuildTask {

typedef HRESULT (WINAPI *ShaderCompilerType)
   (
        LPCSTR                          pSrcData,
        UINT                            SrcDataLen,
        CONST D3DXMACRO*                pDefines,
        LPD3DXINCLUDE                   pInclude,
        LPCSTR                          pFunctionName,
        LPCSTR                          pProfile,
        DWORD                           Flags,
        LPD3DXBUFFER*                   ppShader,
        LPD3DXBUFFER*                   ppErrorMsgs,
        LPD3DXCONSTANTTABLE*            ppConstantTable);

String^
PixelShaderCompile::GetDxLibraryToUse()
{
	if (!_calculatedDxLibraryToUse)
	{		
		const int maxIndex = 60;
		const int minIndex = 36;
		bool gotOne = false;
		int index = maxIndex;

		// DirectX SDK's install new D3DX libraries by appending a version number.  Here, we start from a high
		// number and count down until we find a library that contains the function we're looking for.  This
		// thus gets us the most recently installed SDK.  If there are no SDKs installed, we _dxLibraryToUse remains
		// NULL and we'll use the statically linked one.
		for (int index = maxIndex; !gotOne && index >= minIndex; index--)
		{
			String^ libName = "d3dx9_" + index.ToString() + ".dll";
			CString libNameAsCString(libName);

			HMODULE dxLibrary = ::LoadLibrary((LPCWSTR)libNameAsCString);
			if (dxLibrary != NULL)
			{
				FARPROC sc = ::GetProcAddress(dxLibrary, "D3DXCompileShader");
				if (sc != NULL)
				{
					gotOne = true;
					_dxLibraryToUse = libName;
				}
				::FreeLibrary(dxLibrary);
			}
		}

		_calculatedDxLibraryToUse = true;
	}

	return _dxLibraryToUse;
}

bool
PixelShaderCompile::Execute()
{
	marshal_context^ context = gcnew marshal_context();
	this->_outputs = gcnew List<ITaskItem ^>();

	// Continue through all sources, even if some fail.  Keep track if we failed.
	bool anyFailed = false;

	for (int i = 0; i < this->Sources->Length; i++)
	{
		ITaskItem^ ti = this->Sources[i];

		if (!File::Exists(ti->ItemSpec))
		{
			Log->LogError("ResourceNotFound {0}:", ti->ItemSpec);
			anyFailed = true;
		}
		else
		{
			String^ shaderText = File::ReadAllText(ti->ItemSpec);

			LPCSTR shaderTextLPCSTR = context->marshal_as<LPCSTR>(shaderText);
			LPD3DXBUFFER compiledShader;
			LPD3DXBUFFER errorMessages;

			int size = shaderText->Length;

			ShaderCompilerType shaderCompiler = ::D3DXCompileShader;

			// Try to get the latest if the DX SDK is installed.  Otherwise, back up to the statically linked version.
			String^ libraryToLoad = GetDxLibraryToUse();
			CString libraryToLoadAsCString(libraryToLoad);

			HMODULE dxLibrary = ::LoadLibrary((LPCWSTR)libraryToLoadAsCString); 
			bool gotDynamicOne = false;
			if (dxLibrary != NULL)
			{
				FARPROC sc = ::GetProcAddress(dxLibrary, "D3DXCompileShader");
				shaderCompiler = (ShaderCompilerType)sc;
				gotDynamicOne = true;
			}

			HRESULT compileResult = 
				shaderCompiler(
					shaderTextLPCSTR, 
					size, 
					NULL, // pDefines
					NULL, // pIncludes
					"main", // entrypoint
					"ps_2_0", // profile, always PS 2.0
					0, // compiler flags
					&compiledShader,
					&errorMessages,
					NULL   // constant table output
					);
			
			if (!SUCCEEDED(compileResult))
			{
				char *nativeErrorString = (char *)(errorMessages->GetBufferPointer());
				String^ managedErrorString = context->marshal_as<String^>(nativeErrorString);

				// Need to build up our own error information, since error string from the compiler
				// doesn't identify the source file.

				// Pull the error string from the shader compiler apart.
				// Note that the backslashes are escaped, since C++ needs an escaping of them.  

				String^ subcategory = "Shader";
				String^ dir;
				String^ line;
				String^ col;
				String^ descrip;
				String^ file;
				String^ errorCode = "";
				String^ helpKeyword = "";
				int     lineNum = 0;
				int     colNum = 0;
				bool    parsedLineNum = false;

				if (gotDynamicOne)
				{
					String^ regexString = "(?<directory>[^@]+)memory\\((?<line>[^@]+),(?<col>[^@]+)\\): (?<descrip>[^@]+)";
					Regex^ errorRegex = gcnew Regex(regexString);
					Match^ m = errorRegex->Match(managedErrorString);

					dir     = m->Groups["directory"]->Value;
					line    = m->Groups["line"]->Value;
					col     = m->Groups["col"]->Value;
					descrip = m->Groups["descrip"]->Value;
					file    = String::Concat(dir, ti->ItemSpec);

					parsedLineNum = Int32::TryParse(line, lineNum);
					Int32::TryParse(col, colNum);
				}
				else
				{
					// Statically linked d3dx9.lib's error string is a different format, need to parse that.

					// Example string: (16): error X3018: invalid subscript 'U'
					String^ regexString = "\\((?<line>[^@]+)\\): (?<descrip>[^@]+)";
					Regex^ errorRegex = gcnew Regex(regexString);
					Match^ m = errorRegex->Match(managedErrorString);

					line    = m->Groups["line"]->Value;
					descrip = m->Groups["descrip"]->Value;
					file = ti->ItemSpec;

					parsedLineNum = Int32::TryParse(line, lineNum);

					int colNum = 0;  // no column information supplied
				}

				if (!parsedLineNum)
				{
					// Just use the whole string as the description.
					descrip = managedErrorString;
				}

				Log->LogError(subcategory, errorCode, helpKeyword, file, lineNum, colNum, lineNum, colNum, "{0}", descrip);

				anyFailed = true;

			}
			else
			{
				// Create the output task item for the current input task item using the same name, but changing
				// the extension to ps (PixelShader).
				TaskItem^ output = gcnew TaskItem(Path::ChangeExtension(ti->ItemSpec, ".ps"));

    			char *nativeBytestream = (char *)(compiledShader->GetBufferPointer());
				array<unsigned char>^ arr = gcnew array<unsigned char>(compiledShader->GetBufferSize());

				// TODO: Really ugly way to copy from a uchar* to a managed array, but I can't easily figure out the
				// "right" way to do it.
				for (int i = 0; i < compiledShader->GetBufferSize(); i++)
				{
					arr[i] = nativeBytestream[i];
				}

				File::WriteAllBytes(output->ItemSpec, arr);
				
				this->_outputs->Add(output);
				Log->LogMessage("Source: {0} Target: {1}", ti->ItemSpec, output->ItemSpec);
			}

			if (dxLibrary != NULL)
			{
				::FreeLibrary(dxLibrary);
			}
		}
	}  

	return !anyFailed;
}

}