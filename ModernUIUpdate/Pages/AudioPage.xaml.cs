using CharacterEditor.Models;
using CharacterEditor.ViewModels;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharacterEditor.Pages
{
    /// <summary>
    /// Interaction logic for FramesheetPage.xaml
    /// </summary>
    public partial class AudioPage : UserControl
    {
        private FrameSheetVM viewModel;
        private Timer previewTimer;

        public AudioPage()
        {
            InitializeComponent();
        }

        private void addAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            addAnimationPopup.IsOpen = true;
        }

        private void showSheetInfoButton_Click(object sender, RoutedEventArgs e)
        {
            showSheetInfoPopup.IsOpen = true;
        }

        private bool DrawMarkers()
        {
            try
            {
                if (viewModel.SelectedProject.SelectedCharacter.FrameHeight < 10 || viewModel.SelectedProject.SelectedCharacter.FrameWidth < 10 || viewModel.SelectedProject.SelectedCharacter.SelectedAnimation == null)
                {
                    return false;
                }

                IAnimationBase selectedAnimationBase = viewModel.SelectedProject.SelectedCharacter.SelectedAnimation;

                if (selectedAnimationBase.GetType() != typeof(SimpleAnimation))
                {
                    return false;
                }

                SimpleAnimation selectedAnimation = selectedAnimationBase as SimpleAnimation;

                Brush gridBlackoutBrush = new SolidColorBrush(Colors.Black) { Opacity = 0.75 };

                for (int i = 0; i < selectedAnimation.RangeStart; i++)
                {
                    if (
                        i >=
                        GetMaxPossiblePosition(viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                        viewModel.SelectedProject.SelectedCharacter.FrameWidth, viewModel.SelectedProject.SelectedCharacter.FrameHeight, viewModel.SelectedProject.SelectedCharacter.LeftMargin, viewModel.SelectedProject.SelectedCharacter.UpperMargin, viewModel.SelectedProject.SelectedCharacter.RightMargin, viewModel.SelectedProject.SelectedCharacter.BottomMargin)
                        )
                    {
                        break;
                    }

                    Int32Rect posRect = GetPositionRectangle(viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                        viewModel.SelectedProject.SelectedCharacter.FrameWidth, viewModel.SelectedProject.SelectedCharacter.FrameHeight, viewModel.SelectedProject.SelectedCharacter.LeftMargin, viewModel.SelectedProject.SelectedCharacter.UpperMargin, viewModel.SelectedProject.SelectedCharacter.RightMargin, viewModel.SelectedProject.SelectedCharacter.BottomMargin, i);

                    Rectangle tempRectangle = new Rectangle()
                    {
                        Fill = gridBlackoutBrush,
                        StrokeThickness = 0,
                        Width = posRect.Width,
                        Height = posRect.Height
                    };

                    Canvas.SetLeft(tempRectangle, posRect.X);
                    Canvas.SetTop(tempRectangle, posRect.Y);

                    canvas.Children.Add(tempRectangle);
                }
                for (int i = selectedAnimation.RangeEnd + 1; i < GetMaxPossiblePosition(viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                        viewModel.SelectedProject.SelectedCharacter.FrameWidth, viewModel.SelectedProject.SelectedCharacter.FrameHeight, viewModel.SelectedProject.SelectedCharacter.LeftMargin, viewModel.SelectedProject.SelectedCharacter.UpperMargin, viewModel.SelectedProject.SelectedCharacter.RightMargin, viewModel.SelectedProject.SelectedCharacter.BottomMargin); i++)
                {
                    Int32Rect posRect = GetPositionRectangle(viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                        viewModel.SelectedProject.SelectedCharacter.FrameWidth, viewModel.SelectedProject.SelectedCharacter.FrameHeight, viewModel.SelectedProject.SelectedCharacter.LeftMargin, viewModel.SelectedProject.SelectedCharacter.UpperMargin, viewModel.SelectedProject.SelectedCharacter.RightMargin, viewModel.SelectedProject.SelectedCharacter.BottomMargin, i);

                    Rectangle tempRectangle = new Rectangle()
                    {
                        Fill = gridBlackoutBrush,
                        StrokeThickness = 0,
                        Width = posRect.Width,
                        Height = posRect.Height
                    };

                    Canvas.SetLeft(tempRectangle, posRect.X);
                    Canvas.SetTop(tempRectangle, posRect.Y);

                    canvas.Children.Add(tempRectangle);
                }


                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
                throw;
            }
        }

        private bool DrawLines()
        {
            try
            {
                if (viewModel.SelectedProject.SelectedCharacter.FrameHeight < 10 || viewModel.SelectedProject.SelectedCharacter.FrameWidth < 10)
                {
                    return false;
                }

                Brush gridLineBrushNormal = new LinearGradientBrush()
                {
                    SpreadMethod = GradientSpreadMethod.Repeat,
                    StartPoint = new Point(0.1, 0.1),
                    EndPoint = new Point(0.2, 0.2),
                    GradientStops = new GradientStopCollection(new List<GradientStop> { new GradientStop(Colors.Black, 0.5), new GradientStop(Colors.White, 0.5) })
                };

                Brush gridLineBrushMargin = new LinearGradientBrush()
                {
                    SpreadMethod = GradientSpreadMethod.Repeat,
                    StartPoint = new Point(0.1, 0.1),
                    EndPoint = new Point(0.2, 0.2),
                    GradientStops = new GradientStopCollection(new List<GradientStop> { new GradientStop(Colors.Red, 0.5), new GradientStop(Colors.Blue, 0.5) })
                };

                //Binding bindingWidth = new Binding("Width");
                //bindingWidth.Source = viewModel.SelectedProject.SelectedCharacter.FrameSheet;

                //Binding bindingHeight = new Binding("Height");
                //bindingHeight.Source = viewModel.SelectedProject.SelectedCharacter.FrameSheet;

                for (int iDistance = viewModel.SelectedProject.SelectedCharacter.UpperMargin; iDistance <= viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight
                    - viewModel.SelectedProject.SelectedCharacter.BottomMargin; iDistance += viewModel.SelectedProject.SelectedCharacter.FrameHeight)
                {
                    Brush brushToUse;


                    if (iDistance == viewModel.SelectedProject.SelectedCharacter.UpperMargin)
                    {
                        brushToUse = gridLineBrushMargin;
                    }
                    else if (iDistance > viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight - viewModel.SelectedProject.SelectedCharacter.BottomMargin - viewModel.SelectedProject.SelectedCharacter.FrameHeight)
                    {
                        brushToUse = gridLineBrushMargin;
                    }
                    else
                    {
                        brushToUse = gridLineBrushNormal;
                    }

                    Line tempLine = new Line()
                    {
                        Y1 = iDistance,
                        Y2 = iDistance,
                        X1 = 0,
                        Stroke = brushToUse,
                        StrokeThickness = 1,
                        StrokeDashArray = { 10d, 3d },
                    };
                    //tempLine.SetBinding(Line.X2Property, bindingWidth);
                    //tempLine.SetBinding(Line.X2Property, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth);
                    //tempLine.Width = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth;
                    tempLine.X2 = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth;

                    canvas.Children.Add(tempLine);
                }

                for (int iDistance = viewModel.SelectedProject.SelectedCharacter.LeftMargin; iDistance <= viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth
                    - viewModel.SelectedProject.SelectedCharacter.RightMargin; iDistance += viewModel.SelectedProject.SelectedCharacter.FrameWidth)
                {
                    Brush brushToUse;

                    if (iDistance == viewModel.SelectedProject.SelectedCharacter.LeftMargin)
                    {
                        brushToUse = gridLineBrushMargin;
                    }
                    else if (iDistance > viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth - viewModel.SelectedProject.SelectedCharacter.RightMargin - viewModel.SelectedProject.SelectedCharacter.FrameWidth)
                    {
                        brushToUse = gridLineBrushMargin;
                    }
                    else
                    {
                        brushToUse = gridLineBrushNormal;
                    }

                    Line tempLine = new Line()
                    {
                        X1 = iDistance,
                        X2 = iDistance,
                        Y1 = 0,
                        Stroke = brushToUse,
                        StrokeThickness = 1,
                        StrokeDashArray = { 10d, 3d },
                    };
                    //tempLine.SetBinding(Line.Y2Property, bindingHeight);
                    //tempLine.Height = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight;
                    tempLine.Y2 = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight;

                    canvas.Children.Add(tempLine);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }

        private bool DrawMargins()
        {
            try
            {
                int bMargin = viewModel.SelectedProject.SelectedCharacter.BottomMargin;
                int uMargin = viewModel.SelectedProject.SelectedCharacter.UpperMargin;
                int lMargin = viewModel.SelectedProject.SelectedCharacter.LeftMargin;
                int rMargin = viewModel.SelectedProject.SelectedCharacter.RightMargin;
                int imageHeight = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight;
                int imageWidth = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth;
                int frameHeight = viewModel.SelectedProject.SelectedCharacter.FrameHeight;
                int frameWidth = viewModel.SelectedProject.SelectedCharacter.FrameWidth;

                if (frameHeight < 10 || frameWidth < 10 ||
                    uMargin < 0 || lMargin < 0 || rMargin < 0 || bMargin < 0 ||
                    imageHeight < 0 || imageWidth < 0 ||
                    viewModel.SelectedProject.SelectedCharacter.SelectedAnimation == null)
                {
                    return false;
                }

                Brush gridBlackoutBrush = new SolidColorBrush(Colors.Black) { Opacity = 0.25 };

                Rectangle upperRectangle = new Rectangle()
                {
                    Fill = gridBlackoutBrush,
                    StrokeThickness = 0,
                    Width = imageWidth,
                    Height = uMargin
                };

                Canvas.SetLeft(upperRectangle, 0);
                Canvas.SetTop(upperRectangle, 0);

                Rectangle leftRectangle = new Rectangle()
                {
                    Fill = gridBlackoutBrush,
                    StrokeThickness = 0,
                    Width = lMargin,
                    Height = imageHeight - uMargin - bMargin
                };

                Canvas.SetLeft(leftRectangle, 0);
                Canvas.SetTop(leftRectangle, uMargin);

                Rectangle rightRectangle = new Rectangle()
                {
                    Fill = gridBlackoutBrush,
                    StrokeThickness = 0,
                    Width = rMargin,
                    Height = imageHeight - uMargin - bMargin
                };

                Canvas.SetLeft(rightRectangle, imageWidth - rMargin);
                Canvas.SetTop(rightRectangle, uMargin);

                Rectangle bottomRectangle = new Rectangle()
                {
                    Fill = gridBlackoutBrush,
                    StrokeThickness = 0,
                    Width = imageWidth,
                    Height = bMargin
                };

                Canvas.SetLeft(bottomRectangle, 0);
                Canvas.SetTop(bottomRectangle, imageHeight - bMargin);

                canvas.Children.Add(upperRectangle);
                canvas.Children.Add(leftRectangle);
                canvas.Children.Add(rightRectangle);
                canvas.Children.Add(bottomRectangle);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
                throw;
            }
        }

        private bool DrawSigns()
        {
            try
            {
                int bMargin = viewModel.SelectedProject.SelectedCharacter.BottomMargin;
                int uMargin = viewModel.SelectedProject.SelectedCharacter.UpperMargin;
                int lMargin = viewModel.SelectedProject.SelectedCharacter.LeftMargin;
                int rMargin = viewModel.SelectedProject.SelectedCharacter.RightMargin;
                int imageHeight = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight;
                int imageWidth = viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth;
                int frameHeight = viewModel.SelectedProject.SelectedCharacter.FrameHeight;
                int frameWidth = viewModel.SelectedProject.SelectedCharacter.FrameWidth;

                if (frameHeight < 10 || frameWidth < 10 ||
                    uMargin < 0 || lMargin < 0 || rMargin < 0 || bMargin < 0 ||
                    imageHeight < 0 || imageWidth < 0 ||
                    viewModel.SelectedProject == null ||
                    viewModel.SelectedProject.SelectedCharacter == null ||
                    viewModel.SelectedProject.SelectedCharacter.SelectedAnimation == null)
                {
                    return false;
                }

                int startIndex = (viewModel.SelectedProject.SelectedCharacter.SelectedAnimation as SimpleAnimation).StartIndex;
                int endIndex = (viewModel.SelectedProject.SelectedCharacter.SelectedAnimation as SimpleAnimation).EndIndex;

                TextBlock startText = new TextBlock()
                {
                    Text = "S",
                    Foreground = Brushes.Blue
                };


                Int32Rect startPlacement = GetPositionRectangle(imageWidth, imageHeight, frameWidth, frameHeight, lMargin, uMargin, rMargin, bMargin, startIndex);

                Canvas.SetLeft(startText, startPlacement.X);
                Canvas.SetTop(startText, startPlacement.Y);


                TextBlock endText = new TextBlock()
                {
                    Text = "E",
                    Foreground = Brushes.Red
                };

                Int32Rect endPlacement = GetPositionRectangle(imageWidth, imageHeight, frameWidth, frameHeight, lMargin, uMargin, rMargin, bMargin, endIndex);

                Canvas.SetLeft(endText, endPlacement.X);
                Canvas.SetTop(endText, endPlacement.Y);

                canvas.Children.Add(startText);
                canvas.Children.Add(endText);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
                throw;
            }
        }


        private void BeginDrawing()
        {
            canvas.Children.RemoveRange(1, canvas.Children.Count - 1);
            DrawLines();
            DrawMarkers();
            DrawMargins();
            DrawSigns();
        }

        IFrameBase activeFrame;
        int currentStep;
        TimeSpan duration;
        DateTime frameStart;

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as FrameSheetVM;
            viewModel.PropertyChanged += viewModel_PropertyChanged;

            previewTimer = new Timer(100d);
            previewTimer.Elapsed += previewTimer_Elapsed;
            previewTimer.Start();

            BeginDrawing();
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedProject.SelectedCharacter":
                case "SelectedProject.SelectedCharacter.FrameWidth":
                case "SelectedProject.SelectedCharacter.FrameHeight":
                case "SelectedProject.SelectedCharacter.LeftMargin":
                case "SelectedProject.SelectedCharacter.UpperMargin":
                case "SelectedProject.SelectedCharacter.RightMargin":
                case "SelectedProject.SelectedCharacter.BottomMargin":
                case "SelectedProject.SelectedCharacter.FrameSheet":
                case "SelectedProject.SelectedCharacter.SelectedAnimation.RangeStart":
                    {
                        if (e.PropertyName == "SelectedProject.SelectedCharacter" && (viewModel.SelectedProject == null || viewModel.SelectedProject.SelectedCharacter == null))
                        {
                            previewImage.Source = null;
                            return;
                        }
                        // Drawing the lines for the Grid acording to the possible new sizes.
                        BeginDrawing();
                        break;
                    }
                case "SelectedProject.SelectedCharacter.SelectedAnimation.Frames.Collection":
                    {

                        if (viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames.Count > 0)
                        {
                            break;
                        }
                        activeFrame = viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames[0];
                        duration = activeFrame.Duration;
                        frameStart = DateTime.Now;
                        currentStep += 1;
                        break;
                    }
                default:
                    break;
            }
        }

        void previewTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (frameStart.Add(duration) > DateTime.Now)
            {
                return;
            }

            if (viewModel.SelectedProject == null || viewModel.SelectedProject.SelectedCharacter == null || viewModel.SelectedProject.SelectedCharacter.FrameSheet == null || viewModel.SelectedProject.SelectedCharacter.SelectedAnimation == null || viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames.Count <= 0 || viewModel.SelectedProject.SelectedCharacter.FrameWidth <= 0 || viewModel.SelectedProject.SelectedCharacter.FrameHeight <= 0)
            {
                return;
            }

            if (currentStep >= viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames.Count)
            {
                currentStep = 0;
            }

            activeFrame = viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames.FirstOrDefault((f) => f.Step == currentStep);
            if (activeFrame == null)
            {
                return;
            }
            try
            {

                viewModel.SelectedProject.SelectedCharacter.FrameSheet.Dispatcher.Invoke(
                    () =>
                    {
                        try
                        {
                            currentStep += 1;
                            duration = activeFrame.Duration;
                            frameStart = DateTime.Now;

                            WriteableBitmap frame;

                            if (viewModel.SelectedProject.SelectedCharacter == null || viewModel.SelectedProject.SelectedCharacter.FrameWidth > viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth || viewModel.SelectedProject.SelectedCharacter.FrameHeight > viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight)
                            {
                                return;
                            }

                            int width = viewModel.SelectedProject.SelectedCharacter.FrameWidth;
                            int height = viewModel.SelectedProject.SelectedCharacter.FrameHeight;
                            int lMarge = viewModel.SelectedProject.SelectedCharacter.LeftMargin;
                            int uMarge = viewModel.SelectedProject.SelectedCharacter.UpperMargin;
                            int rMarge = viewModel.SelectedProject.SelectedCharacter.RightMargin;
                            int bMarge = viewModel.SelectedProject.SelectedCharacter.BottomMargin;
                            int position = activeFrame.Position;

                            int stride = width * (viewModel.SelectedProject.SelectedCharacter.FrameSheet.Format.BitsPerPixel / 8);

                            frame = new WriteableBitmap(
                           width, height,
                           viewModel.SelectedProject.SelectedCharacter.FrameSheet.DpiX, viewModel.SelectedProject.SelectedCharacter.FrameSheet.DpiY,
                           viewModel.SelectedProject.SelectedCharacter.FrameSheet.Format, viewModel.SelectedProject.SelectedCharacter.FrameSheet.Palette
                           );
                            byte[] data = new byte[stride * height];

                            Int32Rect posRect = GetPositionRectangle(viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth, viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                                width, height, lMarge, uMarge, rMarge, bMarge, position);
                            viewModel.SelectedProject.SelectedCharacter.FrameSheet.CopyPixels(posRect, data, stride, 0);

                            frame.Lock();
                            frame.WritePixels(new Int32Rect(0, 0, width, height),
                  data, stride, 0);
                            frame.Unlock();
                            previewImage.Source = frame;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Debugger.Break();
                            activeFrame = viewModel.SelectedProject.SelectedCharacter.SelectedAnimation.Frames[0];

                        }
                    }
                    );
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine("TaskCanceledException happened in a dispatcher in mainwindow. " + ex.Message);
            }
        }

        private int GetMaxPossiblePosition(int argImageWidth, int argImageHeight, int argFrameWidth, int argFrameHeight, int argLeftMargin, int argUpperMargin, int argRightMargin, int argBottomMargin)
        {
            int newWidthRange = argImageWidth - argLeftMargin - argRightMargin;
            int newHeightRange = argImageHeight - argBottomMargin - argUpperMargin;
            int xCount = newWidthRange / argFrameWidth;
            int yCount = newHeightRange / argFrameHeight;
            return xCount * yCount;
        }

        private int GetPositionByCoordinates(int argImageWidth, int argImageHeight, int argFrameWidth, int argFrameHeight, int argLeftMargin, int argUpperMargin, int argRightMargin, int argBottomMargin, int xCoordinate, int yCoordinate)
        {
            if (
                xCoordinate < argLeftMargin || xCoordinate > argImageWidth - (argRightMargin / argFrameWidth + Math.Min(1, argRightMargin % argFrameWidth)) * argFrameWidth ||
                yCoordinate < argUpperMargin || yCoordinate > argImageHeight - (argBottomMargin / argFrameHeight + Math.Min(1, argBottomMargin % argFrameHeight)) * argFrameHeight
                )
            {
                return 0;
            }

            int preres = GetMaxPossiblePosition(argImageWidth, yCoordinate, argFrameWidth, argFrameHeight, argLeftMargin, argUpperMargin, argRightMargin, 0);

            Int32Rect calculateEarly = GetPositionRectangle(argImageWidth, argImageHeight, argFrameWidth, argFrameHeight, argLeftMargin, argUpperMargin, argRightMargin, 0, preres);

            preres += GetMaxPossiblePosition(xCoordinate, calculateEarly.Height, argFrameWidth, argFrameHeight, argLeftMargin, 0, 0, 0);

            return preres;
        }

        private Int32Rect GetPositionRectangle(int argImageWidth, int argImageHeight, int argFrameWidth, int argFrameHeight, int argLeftMargin, int argUpperMargin, int argRightMargin, int argBottomMargin, int argPosition)
        {
            int heightPos = argUpperMargin;
            int stepcount = argPosition;

            int rowCount = (argImageWidth - argLeftMargin - argRightMargin) / argFrameWidth;
            int columnCount = (argImageHeight - argUpperMargin - argBottomMargin) / argFrameHeight;

            while (stepcount >= rowCount)
            {
                stepcount -= rowCount;
                heightPos += argFrameHeight;
            }

            int widthPos = argLeftMargin + stepcount * argFrameWidth;

            if (heightPos + argFrameHeight + argUpperMargin + argBottomMargin > argImageHeight)
            {
                return new Int32Rect(0, 0, argFrameWidth, argFrameHeight);
            }

            return new Int32Rect(widthPos, heightPos, argFrameWidth, argFrameHeight);
        }

        private void addAnimationConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            addAnimationPopup.IsOpen = false;
        }

        private void deleteAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimationPopup.IsOpen = true;
        }

        private void deleteAnimationConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimationPopup.IsOpen = false;
        }



        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Point res = e.GetPosition(canvas);

                int newIndex = GetPositionByCoordinates(
                    viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelWidth,
                    viewModel.SelectedProject.SelectedCharacter.FrameSheet.PixelHeight,
                    viewModel.SelectedProject.SelectedCharacter.FrameWidth,
                    viewModel.SelectedProject.SelectedCharacter.FrameHeight,
                    viewModel.SelectedProject.SelectedCharacter.LeftMargin,
                    viewModel.SelectedProject.SelectedCharacter.UpperMargin,
                    viewModel.SelectedProject.SelectedCharacter.RightMargin,
                    viewModel.SelectedProject.SelectedCharacter.BottomMargin,
                    Convert.ToInt32(res.X),
                    Convert.ToInt32(res.Y)
                    );

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    (viewModel.SelectedProject.SelectedCharacter.SelectedAnimation as SimpleAnimation).EndIndex = newIndex;
                }
                else
                {
                    (viewModel.SelectedProject.SelectedCharacter.SelectedAnimation as SimpleAnimation).StartIndex = newIndex;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
               // Debugger.Break();
            }
        }
    }
}
