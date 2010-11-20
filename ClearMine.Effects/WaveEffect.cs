namespace ClearMine.Effects
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    /// <summary>
    /// 
    /// </summary>
    public class WaveEffect : ShaderEffect
    {
        #region Dependency Properties
        /// <summary>
        /// Dependency property which modifies the SwirlStrength variable within the pixel shader.
        /// </summary>
        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(WaveEffect), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));

        /// <summary>
        /// Dependency property for the shader sampler.
        /// </summary>
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(WaveEffect), 0);

        #endregion

        #region Member Data

        /// <summary>
        /// The pixel shader instance.
        /// </summary>
        private static PixelShader pixelShader;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a PixelShader by loading the bytecode.
        /// </summary>
        static WaveEffect()
        {
            pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/ClearMine.Effects;component/ShaderSource/Wave.ps", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Creates the and updates the registered values defined within the pixel shader using the default values.
        /// </summary>
        public WaveEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(StrengthProperty);
            UpdateShaderValue(InputProperty);
        }

        #endregion

        /// <summary>
        /// Gets or sets Stength variable within the shader.
        /// </summary>
        public double Strength
        {
            get { return (double)GetValue(StrengthProperty); }
            set { SetValue(StrengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Input variable within the shader.
        /// </summary>
        [BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}