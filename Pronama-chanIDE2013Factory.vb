Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Utilities

Imports EnvDTE
Imports EnvDTE80
Imports Microsoft.VisualStudio.Shell

''' <summary>
''' Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
''' that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
''' </summary>
<Export(GetType(IWpfTextViewCreationListener))>
<ContentType("text")>
<TextViewRole(PredefinedTextViewRoles.Document)>
NotInheritable Class EditorAdornmentFactory 
    Implements IWpfTextViewCreationListener

    <Import()>
    Friend Property ServiceProvider As SVsServiceProvider

    ''' <summary>
    ''' Defines the adornment layer for the public corner box adornment. This layer is ordered 
    ''' after the selection layer in the Z-order
    ''' </summary>
    <Export(GetType(AdornmentLayerDefinition))>
    <Name("Pronama_chanIDE2013")>
    <Order(Before:=PredefinedAdornmentLayers.Caret)>
    Public _editorAdornmentLayer As AdornmentLayerDefinition

    Private TempPronama_chanIDE2013 As Pronama_chanIDE2013

	''' <summary>
	''' Creates a Pronama_chanIDE2013 manager when a textview is created
	''' </summary>
	''' <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
	Public Sub TextViewCreated(ByVal textView As IWpfTextView) Implements IWpfTextViewCreationListener.TextViewCreated

        TempPronama_chanIDE2013 = New Pronama_chanIDE2013(textView)

        Dim dte As DTE
        dte = CType(ServiceProvider.GetService(GetType(DTE)), DTE)
        AddHandler dte.Events.BuildEvents.OnBuildProjConfigDone, AddressOf foo

    End Sub

    Private Sub foo(ByVal Project As String, ByVal ProjectConfig As String, ByVal Platform As String, ByVal SolutionConfig As String, ByVal Success As Boolean)

        If Me.TempPronama_chanIDE2013 IsNot Nothing Then
            TempPronama_chanIDE2013.Shell.ChangeFace(Success)
        End If

    End Sub

End Class
