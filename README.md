# LeonDirectUI

* 一款轻巧的DirectUI框架；
* 支持虚拟控件级别的鼠标事件；
* 可以自定义布局并在容器尺寸改变时设置响应布局；
* 界面图文可以自定义绘制样式和对齐方式；
* 基因决定支持容器克隆并引用同一组虚拟控件；
* 可以自定义绘制器实现个性化风格定制；
* 小巧简洁，使用方便；
* 底层绘制代码高效，性能资源占用较小；

***
> ### 解决方案结构：

> * ControlBase : 
    
虚拟控件基类，实现了控件的基础属性，包括位置、大小、图文绘制样式、图文对齐方式、响应鼠标事件、公开给访问者的方法等；
可以继承此类以定制功能更强大的控件类型；

> * ContainerBase : 
    
容器基类，继承自 System.Windows.Forms.Control，可直接放置于 WinForm 界面内；
用于提供虚拟控件管理作用；
可以继承此类以扩展更强大的管理容器；

> * CloneContainerBase : 

克隆容器基类，设置克隆的目标容器即可在克隆类中实时响应目标容器的所有效果；

> * PainterBase : 

绘制器基类，用于提供基础的底层绘制方法；
可继承此类定制喜欢的界面风格；

![](https://raw.github.com/CuteLeon/LeonDirectUI/master/README/解决方案结构.png)

***
> ### 演示效果：

> * 自定义响应式布局：

![](https://raw.github.com/CuteLeon/LeonDirectUI/master/README/截图1.png)

> * 克隆容器与目标容器实时响应事件：

![](https://raw.github.com/CuteLeon/LeonDirectUI/master/README/截图2.png)
