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
        #region 绘制 Image 相关

        public static readonly ContentAlignment AnyRightAlign = (ContentAlignment)1092;
        public static readonly ContentAlignment AnyLeftAlign = (ContentAlignment)273;
        public static readonly ContentAlignment AnyTopAlign = (ContentAlignment)7;
        public static readonly ContentAlignment AnyBottomAlign = (ContentAlignment)1792;
        public static readonly ContentAlignment AnyMiddleAlign = (ContentAlignment)112;
        public static readonly ContentAlignment AnyCenterAlign = (ContentAlignment)546;

        [ThreadStatic]
        private static ImageAttributes disabledImageAttr;

        #endregion

        /// <summary>
        /// 绘制虚拟控件
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="control"></param>
        public abstract void Paint(Graphics graphics, ControlBase control);

        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="backColor"></param>
        /// <param name="backgroundImageLayout"></param>
        /// <param name="bounds"></param>
        protected static void DrawBackground(Graphics graphics, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");

            //绘制背景颜色
            if (backColor != Color.Transparent)
            {
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, bounds);
                }
            }

            if (backgroundImage == null)
            {
                return;
            }

            //绘制背景图像
            Rectangle ImageRectangle = bounds;
            switch (backgroundImageLayout)
            {
                //非拉伸，重复绘制平铺
                case ImageLayout.Tile:
                    {
                        using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile))
                        {
                            //平移对齐绘制起始坐标
                            textureBrush.TranslateTransform(ImageRectangle.Left, ImageRectangle.Top);
                            graphics.FillRectangle(textureBrush, ImageRectangle);
                        }
                        return;
                    }
                //非拉伸，仅左上角简单绘制一次
                case ImageLayout.None:
                    {
                        ImageRectangle.Size = backgroundImage.Size;
                        //裁剪为图像区域和显示区域的交集
                        ImageRectangle.Intersect(bounds);
                        graphics.DrawImageUnscaledAndClipped(backgroundImage, ImageRectangle);
                        return;
                    }
                //非拉伸，居中显示
                case ImageLayout.Center:
                    {
                        int LeftOffset = (bounds.Width - backgroundImage.Width) / 2;
                        int TopOffset = (bounds.Height - backgroundImage.Height) / 2;
                        ImageRectangle.Size = backgroundImage.Size;
                        ImageRectangle.Offset(LeftOffset, TopOffset);
                        ImageRectangle.Intersect(bounds);
                        //对图像截取后绘制，否则图像显示不全时将从左上角绘制而不是居中
                        Rectangle SourceRectangle = new Rectangle(Math.Max(0, -LeftOffset), Math.Max(0, -TopOffset), ImageRectangle.Width, ImageRectangle.Height);
                        graphics.DrawImage(backgroundImage, ImageRectangle, SourceRectangle, GraphicsUnit.Pixel);
                        return;
                    }
                //保持比例拉伸，居中
                case ImageLayout.Zoom:
                    {
                        //防止图像尺寸为负数时绘制翻转的图像
                        if (ImageRectangle.Width <= 0 || ImageRectangle.Height <= 0) return;

                        float WidthScale = (float)bounds.Width / (float)backgroundImage.Width;
                        float HeightScale = (float)bounds.Height / (float)backgroundImage.Height;
                        if (WidthScale < HeightScale)
                        {
                            ImageRectangle.Height = (int)((double)((float)backgroundImage.Height * WidthScale) + 0.5);
                            ImageRectangle.Y += (bounds.Height - ImageRectangle.Height) / 2;
                        }
                        else
                        {
                            ImageRectangle.Width = (int)((double)((float)backgroundImage.Width * HeightScale) + 0.5);
                            ImageRectangle.X += (bounds.Width - ImageRectangle.Width) / 2;
                        }
                        graphics.DrawImage(backgroundImage, ImageRectangle);
                        return;
                    }
                //拉伸
                case ImageLayout.Stretch:
                    {
                        //防止图像尺寸为负数时绘制翻转的图像
                        if(ImageRectangle.Width>0 && ImageRectangle.Height>0)
                            graphics.DrawImage(backgroundImage, ImageRectangle);
                        return;
                    }
            }
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

            //这里需要测试图像不同 ContentAlignment 的显示效果；
            Rectangle rectangle = CalcImageRenderBounds(bounds, image, alignment, padding);
            
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
        protected static Rectangle CalcImageRenderBounds(Rectangle r, Image image, ContentAlignment align, Padding padding)
        {
            Size size = image.Size;
            int x = r.X + padding.Left;
            int y = r.Y + padding.Top;
            if ((align & AnyRightAlign) != (ContentAlignment)0)
            {
                x = r.X + r.Width - padding.Right - size.Width;
            }
            else if ((align & AnyCenterAlign) != (ContentAlignment)0)
            {
                x = r.X + (r.Width - size.Width-padding.Horizontal) / 2 + padding.Left;
            }
            if ((align & AnyBottomAlign) != (ContentAlignment)0)
            {
                y = r.Y + r.Height - padding.Bottom - size.Height;
            }
            else if ((align & AnyMiddleAlign) != (ContentAlignment)0)
            {
                y = r.Y + (r.Height - padding.Vertical - size.Height) / 2 + padding.Top;
            }
            return new Rectangle(x, y, size.Width, size.Height);
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
                graphics.DrawImageUnscaled(image, imageBounds);
                return;
            }
        }
    }
}
