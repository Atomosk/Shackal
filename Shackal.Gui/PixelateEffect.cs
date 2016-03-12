using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Shackal.Gui
{
    public class PixelateEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(PixelateEffect), 0);

        public static readonly DependencyProperty PixelatedWidthProperty =
            DependencyProperty.Register("PixelatedWidth", typeof(double), typeof(PixelateEffect),
                new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty PixelatedHeightProperty =
            DependencyProperty.Register("PixelatedHeight", typeof(double), typeof(PixelateEffect),
                new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));

        public PixelateEffect()
        {
            this.PixelShader = ShackalShader.ShaderFactory.CreatePixelateEffect();
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(PixelatedWidthProperty);
            UpdateShaderValue(PixelatedHeightProperty);
        }

        /// <summary>
        /// Gets or sets the Input shader sampler.
        /// </summary>
        [System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get
            {
                return (Brush)GetValue(InputProperty);
            }
            set
            {
                SetValue(InputProperty, value);
            }
        }

        /// <summary>
        /// Width in huge pixels count.
        /// </summary>
        public double PixelatedWidth
        {
            get
            {
                return (double)GetValue(PixelatedWidthProperty);
            }
            set
            {
                SetValue(PixelatedWidthProperty, value);
            }
        }

        /// <summary>
        /// Height in huge pixels count.
        /// </summary>
        public double PixelatedHeight
        {
            get
            {
                return (double)GetValue(PixelatedHeightProperty);
            }
            set
            {
                SetValue(PixelatedHeightProperty, value);
            }
        }

    }
}
