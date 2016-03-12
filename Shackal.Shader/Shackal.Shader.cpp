// This is the main DLL file.

#include "stdafx.h"

#include "Shackal.Shader.h"
#include "PixelateShader.hlsl.h"

using namespace System::IO;

PixelShader^ ShackalShader::ShaderFactory::CreatePixelateEffect()
{
	int shaderLength = sizeof(PixelateBytecode) / sizeof(*PixelateBytecode);
	auto shaderBytes = gcnew array<BYTE>(shaderLength);
	System::IntPtr shaderPtr = System::IntPtr((void*)PixelateBytecode);
	System::Runtime::InteropServices::Marshal::Copy(shaderPtr, shaderBytes, 0, shaderLength);
	auto memoryStream = gcnew MemoryStream(shaderBytes);

	auto shader = gcnew PixelShader();
	shader->SetStreamSource(memoryStream);
	return shader;
}
