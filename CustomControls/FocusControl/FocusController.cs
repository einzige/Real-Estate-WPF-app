using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace FocusVMLib
{
    internal static class FocusController
    {
        #region FocusableProperty

        internal static DependencyProperty GetFocusableProperty(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(FocusablePropertyProperty);
        }

        internal static void SetFocusableProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(FocusablePropertyProperty, value);
        }

        internal static readonly DependencyProperty FocusablePropertyProperty =
            DependencyProperty.RegisterAttached(
            "FocusableProperty",
            typeof(DependencyProperty),
            typeof(FocusController),
            new UIPropertyMetadata(null, OnFocusablePropertyChanged));

        static void OnFocusablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            var property = e.NewValue as DependencyProperty;
            if (property == null)
                return;

            element.DataContextChanged += delegate
            {
                CreateHandler(element, property);
            };

            if (element.DataContext != null)
            {
                CreateHandler(element, property);
            }
        }

        static void CreateHandler(DependencyObject element, DependencyProperty property)
        {
            var focusMover = element.GetValue(FrameworkElement.DataContextProperty) as IFocusMover;
            if (focusMover == null)
            {
                var handler = element.GetValue(MoveFocusSinkProperty) as MoveFocusSink;
                if (handler != null)
                {
                    handler.ReleaseReferences();
                    element.ClearValue(MoveFocusSinkProperty);
                }
            }
            else
            {
                var handler = new MoveFocusSink(element as UIElement, property);
                focusMover.MoveFocus += handler.HandleMoveFocus;
                element.SetValue(MoveFocusSinkProperty, handler);
            }
        }

        #endregion // FocusableProperty

        #region MoveFocusSink

        static readonly DependencyProperty MoveFocusSinkProperty = DependencyProperty.RegisterAttached(
            "MoveFocusSink",
            typeof(MoveFocusSink),
            typeof(FocusController),
            new UIPropertyMetadata(null));

        private class MoveFocusSink
        {
            public MoveFocusSink(UIElement element, DependencyProperty property)
            {
                _element = element;
                _property = property;
            }

            internal void HandleMoveFocus(object sender, MoveFocusEventArgs e)
            {
                if (_element == null || _property == null)
                    return;

                var binding = BindingOperations.GetBinding(_element, _property);
                if (binding == null)
                    return;

                if (e.FocusedProperty != binding.Path.Path)
                    return;

                // Delay the call to allow the current batch
                // of processing to finish before we shift focus.
                _element.Dispatcher.BeginInvoke((Action)delegate
                {
                    _element.Focus();
                },
                DispatcherPriority.Background);
            }

            internal void ReleaseReferences()
            {
                _element = null;
                _property = null;
            }

            UIElement _element;
            DependencyProperty _property;
        }

        #endregion // MoveFocusSink
    }
}