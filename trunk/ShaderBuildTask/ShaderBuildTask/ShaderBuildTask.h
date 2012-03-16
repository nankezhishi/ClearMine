// ShaderBuildTask.h

#pragma once

#include "d3dx9.h"

using namespace System;
using namespace Microsoft::Build::Framework;
using namespace Microsoft::Build::Utilities;
using namespace System::IO;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;


namespace ShaderBuildTask {

public ref class PixelShaderCompile : Task
	{
	public:
		PixelShaderCompile()
		{
			_dxLibraryToUse = String::Empty;
			_calculatedDxLibraryToUse = false;
		}


		[Required]
		property array<ITaskItem^>^ Sources;

		[Output]
		property array<ITaskItem^>^ Outputs
		{
			array<ITaskItem^>^ get()
			{
				return _outputs->ToArray();
			}

			void set(array<ITaskItem^>^ value)
			{
				_outputs->AddRange(value);
			}
		};

		virtual bool Execute() override;


	private:
		String^ GetDxLibraryToUse();


		List<ITaskItem^>^ _outputs;
		String^ _dxLibraryToUse;
		bool _calculatedDxLibraryToUse;
	};
}
