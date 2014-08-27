using CharacterEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CharacterEditor.Helper
{
    public class FrameSheetPartConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //TODO: Modern ui change 
              //  WriteableBitmap frame;
                
                //MainVM viewModel = values[0] as MainVM;

               // int width = viewModel.SelectedProject.SelectedCharacter.FrameWidth;
               // int height = viewModel.SelectedProject.SelectedCharacter.FrameHeight;
               // int lMarge = viewModel.SelectedProject.SelectedCharacter.LeftMargin;
               // int uMarge = viewModel.SelectedProject.SelectedCharacter.UpperMargin;
               // int rMarge = viewModel.SelectedProject.SelectedCharacter.RightMargin;
               // int bMarge = viewModel.SelectedProject.SelectedCharacter.BottomMargin;
               // int position = (int)values[1];

               // if (width <= 0 || height <= 0 || viewModel.FrameSheet.PixelWidth < width || viewModel.FrameSheet.PixelHeight < height)
               // {
               //     return null;
               // }

               // int stride = width * (viewModel.FrameSheet.Format.BitsPerPixel / 8);

               // frame = new WriteableBitmap(
               //width, height,
               //viewModel.FrameSheet.DpiX, viewModel.FrameSheet.DpiY,
               //viewModel.FrameSheet.Format, viewModel.FrameSheet.Palette
               //);
               // byte[] data = new byte[stride * height];

               // Int32Rect posRect = GetPositionRectangle(viewModel.FrameSheet.PixelWidth, viewModel.FrameSheet.PixelHeight,
               //     width, height, lMarge, uMarge, rMarge, bMarge, position);
               // viewModel.FrameSheet.CopyPixels(posRect, data, stride, 0);

               // frame.Lock();
               // frame.WritePixels(new Int32Rect(0, 0, width, height), data, stride, 0);
               // frame.Unlock();

           //     return frame;
                return null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return null;
            }

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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
