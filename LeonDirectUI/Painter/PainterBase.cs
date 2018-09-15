using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.Painter
{
    /// <summary>
    /// 绘制器基类
    /// </summary>
    public abstract class PainterBase : IPainter
    {
        [ThreadStatic]
        private static ImageAttributes disabledImageAttr;

        /// <summary>
        /// 绘制虚拟控件
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="control"></param>
        public abstract void Paint(Graphics graphics, ControlBase control);

        /// <summary>
        /// 绘制背景（从.Net框架源代码摘出来的，稍微改动）
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="backColor"></param>
        /// <param name="backgroundImageLayout"></param>
        /// <param name="bounds"></param>
        protected static void DrawBackground(Graphics graphics, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");

            if (backgroundImage == null)
            {
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, bounds);
                }
                return;
            }

            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    graphics.FillRectangle(textureBrush, bounds);
                    return;
                }
            }

            Rectangle rectangle = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
            using (SolidBrush solidBrush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(solidBrush, bounds);
            }

            if (!bounds.Contains(rectangle))
            {
                if (backgroundImageLayout == ImageLayout.Stretch || backgroundImageLayout == ImageLayout.Zoom)
                {
                    rectangle.Intersect(bounds);
                    graphics.DrawImage(backgroundImage, rectangle);
                    return;
                }
                if (backgroundImageLayout == ImageLayout.None)
                {
                    rectangle.Offset(bounds.Location);
                    Rectangle destRect = rectangle;
                    destRect.Intersect(bounds);
                    Rectangle rectangle2 = new Rectangle(Point.Empty, destRect.Size);
                    graphics.DrawImage(backgroundImage, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel);
                    return;
                }
                Rectangle destRect2 = rectangle;
                destRect2.Intersect(bounds);
                Rectangle rectangle3 = new Rectangle(new Point(destRect2.X - rectangle.X, destRect2.Y - rectangle.Y), destRect2.Size);
                graphics.DrawImage(backgroundImage, destRect2, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
                return;
            }
            else
            {
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(backgroundImage, rectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttributes);
                imageAttributes.Dispose();
            }
        }

        /// <summary>
        /// 计算背景图绘制区域（从.Net框架源代码摘出来的，稍微改动）
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="imageLayout"></param>
        /// <returns></returns>
        protected static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
        {
            Rectangle result = bounds;
            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        result.Size = backgroundImage.Size;
                        break;
                    case ImageLayout.Center:
                        {
                            result.Size = backgroundImage.Size;
                            Size size = bounds.Size;
                            if (size.Width > result.Width)
                            {
                                result.X = (size.Width - result.Width) / 2;
                            }
                            if (size.Height > result.Height)
                            {
                                result.Y = (size.Height - result.Height) / 2;
                            }
                            break;
                        }
                    case ImageLayout.Stretch:
                        result.Size = bounds.Size;
                        break;
                    case ImageLayout.Zoom:
                        {
                            Size size2 = backgroundImage.Size;
                            float num = (float)bounds.Width / (float)size2.Width;
                            float num2 = (float)bounds.Height / (float)size2.Height;
                            if (num < num2)
                            {
                                result.Width = bounds.Width;
                                result.Height = (int)((double)((float)size2.Height * num) + 0.5);
                                if (bounds.Y >= 0)
                                {
                                    result.Y = (bounds.Height - result.Height) / 2;
                                }
                            }
                            else
                            {
                                result.Height = bounds.Height;
                                result.Width = (int)((double)((float)size2.Width * num2) + 0.5);
                                if (bounds.X >= 0)
                                {
                                    result.X = (bounds.Width - result.Width) / 2;
                                }
                            }
                            break;
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="image"></param>
        /// <param name=""></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected static void DrawImage(Graphics graphics, Image image, ContentAlignment alignment, Rectangle bounds, Padding padding, bool enabled)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");
            if (image == null) throw new ArgumentNullException("image");

            Rectangle rectangle = CalculateImageRectangle(bounds, image, alignment, padding);
            if (enabled)
                graphics.DrawImageUnscaledAndClipped(image, rectangle);
            else
                DrawImageDisabled(graphics,image,rectangle);
        }

        /// <summary>
        /// 计算图像绘制区域
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="image"></param>
        /// <param name="alignment"></param>
        /// <param name="padding"></param>
        protected static Rectangle CalculateImageRectangle(Rectangle bounds, Image image, ContentAlignment alignment, Padding padding)
        {
            Rectangle result = bounds;

            //标记
            Enum Top = ContentAlignment.TopLeft & ContentAlignment.TopRight;
            Enum Middle = ContentAlignment.MiddleLeft & ContentAlignment.MiddleRight;
            Enum Bottom = ContentAlignment.BottomLeft & ContentAlignment.BottomRight;
            Enum Left = ContentAlignment.TopLeft & ContentAlignment.BottomLeft;
            Enum Center = ContentAlignment.TopCenter & ContentAlignment.BottomCenter;
            Enum Right = ContentAlignment.TopRight & ContentAlignment.BottomRight;

            //计算垂直坐标
            if (alignment.HasFlag(Top))
                result.Y = padding.Top;
            else if (alignment.HasFlag(Middle))
                result.Y = bounds.Y + (bounds.Height - image.Height - padding.Vertical) / 2;
            else if (alignment.HasFlag(Bottom))
                result.Y = bounds.Bottom - padding.Bottom - image.Height;

            //计算水平坐标
            if (alignment.HasFlag(Left))
                result.X = padding.Left;
            else if (alignment.HasFlag(Center))
                result.X = bounds.Left + (bounds.Width - image.Width - padding.Horizontal) / 2;
            else if (alignment.HasFlag(Right))
                result.X = bounds.Right - padding.Right - image.Width;

            return result;
        }

        /// <summary>
        /// 绘制禁用的图像
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="image"></param>
        /// <param name="imageBounds"></param>
        protected static void DrawImageDisabled(Graphics graphics, Image image, Rectangle imageBounds)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            Size size = image.Size;
            if (disabledImageAttr == null)
            {
                float[][] array = new float[5][];
                array[0] = new float[]
                {
                    0.2125f,
                    0.2125f,
                    0.2125f,
                    0f,
                    0f
                };
                array[1] = new float[]
                {
                    0.2577f,
                    0.2577f,
                    0.2577f,
                    0f,
                    0f
                };
                array[2] = new float[]
                {
                    0.0361f,
                    0.0361f,
                    0.0361f,
                    0f,
                    0f
                };
                float[][] arg_80_0 = array;
                int arg_80_1 = 3;
                float[] expr_78 = new float[5];
                expr_78[3] = 1f;
                arg_80_0[arg_80_1] = expr_78;
                array[4] = new float[]
                {
                    0.38f,
                    0.38f,
                    0.38f,
                    0f,
                    1f
                };
                ColorMatrix colorMatrix = new ColorMatrix(array);
                disabledImageAttr = new ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(colorMatrix);
            }

            using (Bitmap bitmap = new Bitmap(image.Width, image.Height))
            {
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    graphics2.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);
                }
                graphics.DrawImageUnscaled(bitmap, imageBounds);
                return;
            }
        }
    }
}
