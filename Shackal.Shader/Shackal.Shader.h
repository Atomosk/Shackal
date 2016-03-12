// Shackal.Shader.h

#pragma once

using namespace System;
using namespace System::Windows::Media::Effects;

namespace ShackalShader {

	public ref class ShaderFactory
	{
		public:
			static PixelShader^ CreatePixelateEffect();
	};
}
