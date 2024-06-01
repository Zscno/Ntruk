# 应用程序标准

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

### 应用程序版本控制

版本名称|版本控制|Git注释
|:-:|:-:|:-:|
Major|重大更新（用户能一眼看出的变化，如更改UI风格）|Refactoring、Update
Minor|普通更新（用户使用中能看出的变化，如增加大功能）|Add、Update
Build|所有补丁（用户很难看出的变化，如完善功能）|Refactoring、Fixed、Update
Revision|非程序更新（与程序本体无关的更新，如完善自述文件）|Add、Refactoring、Update

## 4.API

1. 所有API类型的访问修饰符必须为`internal`。
2. 所有API类型不写注释。
3. 在所有API类型的方法的注释中，关于代码的部分（如关键字或类型）必须以一下形式出现：`<see cref=" 关键字或类型"/>`
4. 一般的API类型的方法的返回注释格式为一个`<see cref="类型名称"/>`类型的对象。
5. 如果有API类型的方法需要通过返回结果确定操作是否成功，那么在方法的返回注释中需要写`<para>（返回*特定值*则操作成功；）返回*特定值*则操作失败。</para>`