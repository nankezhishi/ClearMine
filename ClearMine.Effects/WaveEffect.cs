namespace ClearMine.Effects
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class WaveEffect : ShaderEffect
    {
        #region Dependency Properties
        /// <summary>
        /// Dependency property which modifies the Strength variable within the pixel shader.
        /// </summary>
        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(WaveEffect), new UIPropertyMetadata(0.1, PixelShaderConstantCallback(0)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(WaveEffect), new UIPropertyMetadata(6.28, PixelShaderConstantCallback(1)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(WaveEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(2)));

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
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
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
            UpdateShaderValue(LengthProperty);
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(InputProperty);

            Trace.TraceInformation(ResourceHelper.FindText("NewWavingEffectCreated"));
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
        /// Gets or sets Stength variable within the shader.
        /// </summary>
        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets Stength variable within the shader.
        /// </summary>
        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
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