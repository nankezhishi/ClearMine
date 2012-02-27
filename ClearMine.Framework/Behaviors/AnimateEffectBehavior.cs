namespace ClearMine.Framework.Behaviors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    public static class AnimateEffectBehavior
    {
        private static IList<Storyboard> animations = new List<Storyboard>();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static AnimateEffectBehavior()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
        }

        private static void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("WavingFlag".Equals(e.PropertyName, StringComparison.Ordinal))
            {
                if (Settings.Default.WavingFlag)
                {
                    foreach (var animation in animations)
                    {
                        animation.Resume();
                    }
                }
                else
                {
                    foreach (var animation in animations)
                    {
                        animation.Pause();
                    }
                }
            }
        }

        public static bool GetWaving(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return (bool)obj.GetValue(WavingProperty);
        }

        public static void SetWaving(DependencyObject obj, bool value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.SetValue(WavingProperty, value);
        }

        // Using a DependencyProperty as the backing store for Waving.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WavingProperty =
            DependencyProperty.RegisterAttached("Waving", typeof(bool), typeof(AnimateEffectBehavior), new UIPropertyMetadata(new PropertyChangedCallback(OnWavingPropertyChanged)));

        private static void OnWavingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (Infrastructure.IsInDesignMode)
                return;

            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                var waving = new DoubleAnimationUsingKeyFrames();
                waving.KeyFrames.Add(new LinearDoubleKeyFrame()
                {
                    Value = 6.28,
                    KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 1)),
                });
                Timeline.SetDesiredFrameRate(waving, 12);
                Storyboard.SetTargetProperty(waving, new PropertyPath("(UIElement.Effect).(WaveEffect.Offset)"));
                Storyboard.SetTarget(waving, control);
                var story = new Storyboard()
                {
                    RepeatBehavior = new RepeatBehavior(Int32.MaxValue),
                };
                story.Children.Add(waving);
                animations.Add(story);
                story.Begin();

                Trace.TraceInformation(ResourceHelper.FindText("NewWavingAnimationCreated"));
                if (!Settings.Default.WavingFlag)
                {
                    story.Pause();
                }
            }
        }

        public static string GetAnimationName(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return (string)obj.GetValue(AnimationNameProperty);
        }

        public static void SetAnimationName(DependencyObject obj, string value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.SetValue(AnimationNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnimationName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationNameProperty =
            DependencyProperty.RegisterAttached("AnimationName", typeof(string), typeof(AnimateEffectBehavior), new UIPropertyMetadata(OnAnimationNamePropertyChanged));

        private static void OnAnimationNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                foreach (DictionaryEntry resource in control.Template.Resources)
                {
                    if (resource.Key.Equals(GetAnimationName(control)))
                    {
                        Storyboard storyboard = new Storyboard();
                        var timeLine = resource.Value as Timeline;
                        Storyboard.SetTarget(timeLine, control);
                        storyboard.Children.Add(timeLine);
                        storyboard.Begin();
                        break;
                    }
                }
            }
        }
    }
}
