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
        private static ImageAttributes DisabledImageAttr;

        #endregion

        /// <summary>
        /// 绘制虚拟控件
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="control"></param>
        public abstract void Paint(Graphics graphics, ControlBase control);

        #region 基类方法

        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="graphics">绘制对象</param>
        /// <param name="backgroundImage">背景图（为空时仅绘制背景颜色）</param>
        /// <param name="backColor">背景颜色（为 Transparent 时不绘制）</param>
        /// <param name="backgroundImageLayout">背景图像布局方式</param>
        /// <param name="bounds">绘制区域边界</param>
        protected static void DrawBackground(
            Graphics graphics,
            Image backgroundImage,
            Color backColor,
            ImageLayout backgroundImageLayout,
            Rectangle bounds)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

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
                        Rectangle SourceRectangle = new Rectangle(
                            Math.Max(0, -LeftOffset),
                            Math.Max(0, -TopOffset),
                            ImageRectangle.Width,
                            ImageRectangle.Height);
                        graphics.DrawImage(backgroundImage,
                            ImageRectangle,
                            SourceRectangle,
                            GraphicsUnit.Pixel);
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
                        if (ImageRectangle.Width > 0 && ImageRectangle.Height > 0)
                            graphics.DrawImage(backgroundImage, ImageRectangle);
                        return;
                    }
            }
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="graphics">绘制对象</param>
        /// <param name="image">图像</param>
        /// <param name="alignment">图像对齐方式</param>
        /// <param name="bounds">绘制区域边界</param>
        /// <param name="padding">绘制区域内边框</param>
        /// <param name="enabled">图像样式（为 false 时绘制灰度图像）</param>
        protected static void DrawImage(
            Graphics graphics,
            Image image,
            ContentAlignment alignment,
            Rectangle bounds,
            Padding padding,
            bool enabled = true)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");
            if (image == null) throw new ArgumentNullException("image");
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            //测试 Padding
            Rectangle ImageRectangle = new Rectangle(bounds.Location, image.Size);
            Rectangle SourceRectangle = new Rectangle(Point.Empty, image.Size);
            int LeftOffset = 0, TopOffset = 0;

            //计算绘制区域-横坐标
            if ((alignment & AnyLeftAlign) != (ContentAlignment)0)
            {
                ImageRectangle.X += padding.Left;
            }
            else if ((alignment & AnyRightAlign) != (ContentAlignment)0)
            {
                LeftOffset = bounds.Width - padding.Right - image.Width;
                ImageRectangle.X += LeftOffset;
                if (LeftOffset < 0) SourceRectangle.X = -LeftOffset; //需要裁剪
            }
            else if ((alignment & AnyCenterAlign) != (ContentAlignment)0)
            {
                LeftOffset = (bounds.Width - image.Width - padding.Horizontal) / 2 + padding.Left;
                ImageRectangle.X += LeftOffset;
                if (LeftOffset < 0) SourceRectangle.X = -LeftOffset; //需要裁剪
            }

            //计算绘制区域-纵坐标
            if ((alignment & AnyTopAlign) != (ContentAlignment)0)
            {
                ImageRectangle.Y += padding.Top;
            }
            else if ((alignment & AnyBottomAlign) != (ContentAlignment)0)
            {
                TopOffset = bounds.Height - padding.Bottom - image.Height;
                ImageRectangle.Y += TopOffset;
                if (TopOffset < 0) SourceRectangle.Y = -TopOffset; //需要裁剪
            }
            else if ((alignment & AnyMiddleAlign) != (ContentAlignment)0)
            {
                if (LeftOffset < 0) SourceRectangle.X = -LeftOffset; //需要裁剪

                TopOffset = (bounds.Height - image.Height - padding.Vertical) / 2 + padding.Top;
                ImageRectangle.Y += TopOffset;
                if (TopOffset < 0) SourceRectangle.Y = -TopOffset;
            }

            //最后调整
            ImageRectangle.Intersect(bounds);
            SourceRectangle.Size = ImageRectangle.Size;
            if (SourceRectangle.Width <= 0 || SourceRectangle.Height <= 0) return;

            if (enabled)
            {
                graphics.DrawImage(image, ImageRectangle, SourceRectangle, GraphicsUnit.Pixel);
            }
            else
            {
                DrawImageDisabled(graphics, image, ImageRectangle, SourceRectangle);
            }
        }

        /// <summary>
        /// 绘制禁用的图像
        /// </summary>
        /// <param name="graphics">绘制对象</param>
        /// <param name="image">图像</param>
        /// <param name="imageRectangle">绘制区域</param>
        /// <param name="sourceRectangle">图像剪切区域</param>
        protected static void DrawImageDisabled(
            Graphics graphics,
            Image image,
            Rectangle imageRectangle,
            Rectangle sourceRectangle)
        {
            if (graphics == null) throw new ArgumentNullException("graphics");
            if (image == null) throw new ArgumentNullException("image");
            if (imageRectangle.Width <= 0 || imageRectangle.Height <= 0) return;
            if (sourceRectangle.Width <= 0 || sourceRectangle.Height <= 0) return;

            //懒汉初始化
            if (DisabledImageAttr == null)
            {
                float[][] array = new float[][] {
                    new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
                    new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f },
                    new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
                    new float[] { 0f, 0f, 0f, 1f, 0f },
                    new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f },
                };
                ColorMatrix colorMatrix = new ColorMatrix(array);
                DisabledImageAttr = new ImageAttributes();
                DisabledImageAttr.ClearColorKey();
                DisabledImageAttr.SetColorMatrix(colorMatrix);
            }

            graphics.DrawImage(
                image,
                imageRectangle,
                sourceRectangle.Left,
                sourceRectangle.Top,
                sourceRectangle.Width,
                sourceRectangle.Height,
                GraphicsUnit.Pixel,
                DisabledImageAttr
                );
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        /// <param name="graphics">绘制对象</param>
        /// <param name="text">绘制文本</param>
        /// <param name="font">字体</param>
        /// <param name="forcolor">字体颜色</param>
        /// <param name="align">文本对齐方式</param>
        /// <param name="enabled">是否为可用样式</param>
        /// <param name="showEllipsis">是否显示省略号</param>
        /// <param name="bounds">绘制区域边界</param>
        /// <param name="padding">绘制区域内边界</param>
        protected static void DrawText(
            Graphics graphics,
            string text,
            Font font,
            Color forcolor, 
            ContentAlignment align,
            bool enabled,
            bool showEllipsis,
            Rectangle bounds, 
            Padding padding
            )
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            using (StringFormat stringFormat = CreateStringFormat(align, showEllipsis))
            {
                //根据内边距调整绘制区域
                bounds.Offset(padding.Left, padding.Top);
                bounds.Inflate(-padding.Horizontal,-padding.Vertical);
                if (bounds.Width <= 0 || bounds.Height <= 0) return;

                if (enabled)
                {
                    using (Brush brush = new SolidBrush(forcolor))
                    {
                        graphics.DrawString(text, font, brush, bounds, stringFormat);
                    }
                }
                else
                {
                    //绘制禁用样式的文本，可传入禁用样式的字体颜色
                    ControlPaint.DrawStringDisabled(graphics, text, font, Color.Gray, bounds, stringFormat);
                }
            }
        }

        /// <summary>
        /// 创建文本格式
        /// </summary>
        /// <param name="textAlign"></param>
        /// <param name="showEllipsis"></param>
        /// <returns></returns>
        protected static StringFormat CreateStringFormat(ContentAlignment textAlign, bool showEllipsis)
        {
            StringFormat stringFormat = new StringFormat
            {
                //设置水平对齐方式
                Alignment = TranslateAlignment(textAlign),
                //设置垂直对齐方式
                LineAlignment = TranslateLineAlignment(textAlign)
            };

            //设置省略显示方式
            if (showEllipsis)
            {
                //设置为显示省略号（也可以划词而不显示省略号）
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                stringFormat.FormatFlags |= StringFormatFlags.LineLimit;
            }

            return stringFormat;
        }

        /// <summary>
        /// 转换水平对齐方式
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        protected static StringAlignment TranslateAlignment(ContentAlignment align)
        {
            StringAlignment result;
            if ((align & AnyRightAlign) != (ContentAlignment)0)
            {
                result = StringAlignment.Far;
            }
            else if ((align & AnyCenterAlign) != (ContentAlignment)0)
            {
                result = StringAlignment.Center;
            }
            else
            {
                result = StringAlignment.Near;
            }
            return result;
        }

        /// <summary>
        /// 转换垂直对齐方式
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        protected static StringAlignment TranslateLineAlignment(ContentAlignment align)
        {
            StringAlignment result;
            if ((align & AnyBottomAlign) != (ContentAlignment)0)
            {
                result = StringAlignment.Far;
            }
            else if ((align & AnyMiddleAlign) != (ContentAlignment)0)
            {
                result = StringAlignment.Center;
            }
            else
            {
                result = StringAlignment.Near;
            }
            return result;
        }

        #endregion

    }
}
