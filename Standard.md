# 应用程序项目中的一些固化标准

## 1.Minecraft

资源索引版本号|版本号
|:-:|:-:|
1.19|1.19
1.19|1.19.1
1.19|1.19.2
2   |1.19.3
3   |1.19.4
5   |1.20
5   |1.20.1
8   |1.20.2
12  |1.20.3
12  |1.20.4

## 2.界面或面板

如果要为界面或面板创建**层次**，请使用以下代码：

    <*容器* Background="1f7f7f7f"></*容器*>

如果要为界面或面板创建**大标题**，请使用以下代码：

    <*容器* Height="200">
        <TextBlock FontSize="40" VerticalAlignment="Center"/>`
    </*容器*>

### 每一个界面中的每一个元素必须有`x:Name`属性

面板层次|`x:Name`属性的值
|:-:|:-:|
Page控件下的第一层面板|mainPanel
Page控件下的标题面板|titlePanel
Page控件下的内容面板|contentPanel
容器控件下的内容面板|itemPanel

面板中的元素的`x:Name`属性的值一般为**其面板的第一个单词+控件名缩写（一个单词）**。

## 3.应用程序

应用程序配置文件的路径：`C:\Users\zscNo\AppData\Local\Packages\14929520-b2d0-4c0a-ab07-7df36cf298fe_30zf6cwqqe7zw\LocalState`。

## 4.API

所有API类型的访问修饰符必须为`internal`。

### 名称为`*Helper`的类型

必须有`XML Comments`。且注释内容格式为`关于 *针对的类型* 的 *对本类型中的方法的概括* 操作。`。