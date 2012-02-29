To build this project you need to install:
    1. DirectX SDK 
    2. Shader Build Task. 
        http://wpf.codeplex.com/releases/view/14962
        But this project is targeting VS2008, you need to download its source code and make it targeting VS2010.
        You also need to install VS 2010 SDK to build it.
            http://www.microsoft.com/download/en/details.aspx?id=21835  (Targeting VS2010 SP1) [Recommanded]
            http://www.microsoft.com/download/en/details.aspx?id=2680   (Targeting VS2010)

        Attention: Another shader build task from the following address is not compatible with this project.
        http://code.msdn.microsoft.com/HLSL-Shader-Build-Task-285e9b53

OR, if you just cannot figure out how to make it build. Follow the steps below.
    1. Exclude Mono.fx and Wave.fx
    2. Rename Mono.ps.prebuild to Mono.ps and the same for Wave.ps.prebuild
    3. Change the Build Action of Mono.ps and Wave.ps to "Resource"
    3. You should be able to build it now.