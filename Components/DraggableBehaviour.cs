using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WOZStoryBuilder.Components
{
    public static class DraggableBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(DraggableBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(UIElement element) => (bool)element.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(UIElement element, bool value) => element.SetValue(IsEnabledProperty, value);

        // PositionChanged event
        public static readonly RoutedEvent PositionChangedEvent =
            EventManager.RegisterRoutedEvent(
                "PositionChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DraggableBehavior));

        public static void AddPositionChangedHandler(UIElement element, RoutedEventHandler handler) =>
            element.AddHandler(PositionChangedEvent, handler);

        public static void RemovePositionChangedHandler(UIElement element, RoutedEventHandler handler) =>
            element.RemoveHandler(PositionChangedEvent, handler);

        private class DragState
        {
            public bool IsDragging;
            public Point MouseOffset;
        }

        private static readonly DependencyProperty DragStateProperty =
            DependencyProperty.RegisterAttached("DragState", typeof(DragState), typeof(DraggableBehavior));

        private static DragState GetDragState(UIElement element)
        {
            var state = (DragState)element.GetValue(DragStateProperty);
            if (state == null)
            {
                state = new DragState();
                element.SetValue(DragStateProperty, state);
            }
            return state;
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement element) return;

            if ((bool)e.NewValue)
            {
                element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                element.MouseMove += Element_MouseMove;
                element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
            }
            else
            {
                element.MouseLeftButtonDown -= Element_MouseLeftButtonDown;
                element.MouseMove -= Element_MouseMove;
                element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
            }
        }

        private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;
            if (VisualTreeHelper.GetParent(element) is not Canvas canvas) return;

            var dragState = GetDragState(element);
            dragState.IsDragging = true;
            dragState.MouseOffset = e.GetPosition(element);

            element.CaptureMouse();
            Canvas.SetZIndex(element, 1000);

            e.Handled = true;
        }

        private static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is not UIElement element) return;
            if (!element.IsMouseCaptured) return;
            if (VisualTreeHelper.GetParent(element) is not Canvas canvas) return;

            var dragState = GetDragState(element);
            if (!dragState.IsDragging) return;

            var pos = e.GetPosition(canvas);
            double newLeft = pos.X - dragState.MouseOffset.X;
            double newTop = pos.Y - dragState.MouseOffset.Y;

            // Keep inside canvas
            newLeft = Math.Max(0, Math.Min(canvas.ActualWidth - element.RenderSize.Width, newLeft));
            newTop = Math.Max(0, Math.Min(canvas.ActualHeight - element.RenderSize.Height, newTop));

            Canvas.SetLeft(element, newLeft);
            Canvas.SetTop(element, newTop);

            // Raise PositionChanged event
            element.RaiseEvent(new RoutedEventArgs(PositionChangedEvent, element));
        }

        private static void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;

            var dragState = GetDragState(element);
            dragState.IsDragging = false;

            element.ReleaseMouseCapture();
            Canvas.SetZIndex(element, 1);
        }
    }
}
