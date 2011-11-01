namespace ClearMine.Effects
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using ClearMine.Common.Utilities;

    public class MonoEffect : ShaderEffect
    {
        /// <summary>
        /// Dependency property for the shader sampler.
        /// </summary>
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MonoEffect), 0);

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
        static MonoEffect()
        {
            pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/ClearMine.Effects;component/ShaderSource/Mono.ps", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Creates the and updates the registered values defined within the pixel shader using the default values.
        /// </summary>
        public MonoEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);

            Trace.TraceInformation(LocalizationHelper.FindText("NewMonoEffectCreated"));
        }

        #endregion

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
