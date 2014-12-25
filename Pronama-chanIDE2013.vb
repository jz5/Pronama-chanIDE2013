Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text.Editor

''' <summary>
''' Adornment class that draws a square box in the top right hand corner of the viewport
''' </summary>
Class Pronama_chanIDE2013

    Private WithEvents _view As IWpfTextView
    Friend ReadOnly Shell As Shell
    Private ReadOnly _adornmentLayer As IAdornmentLayer

    Private ShellWidth As Double = 200
    Private ShellHeight As Double = 484.26

    ''' <summary>
    ''' Creates a square image and attaches an event handler to the layout changed event that
    ''' adds the the square in the upper right-hand corner of the TextView via the adornment layer
    ''' </summary>
    ''' <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
    Public Sub New(ByVal view As IWpfTextView)


        _view = view
        Shell = New Shell
        Shell.Opacity = 0.35

        Dim opactiy As Double
        If Double.TryParse(Environment.GetEnvironmentVariable("PRONAMA-CHAN_IDE_OPACITY", EnvironmentVariableTarget.User), opactiy) Then
            If 0 < opactiy AndAlso opactiy <= 1 Then
                Shell.Opacity = opactiy
            End If
        End If

        Dim height As Double
        If Double.TryParse(Environment.GetEnvironmentVariable("PRONAMA-CHAN_IDE_HEIGHT", EnvironmentVariableTarget.User), height) Then
            If height > 0 Then
                ShellWidth *= height / ShellHeight
                ShellHeight = height

                Shell.Width = ShellWidth
                Shell.Height = ShellHeight
            End If
        End If


        'Grab a reference to the adornment layer that this adornment should be added to
        _adornmentLayer = view.GetAdornmentLayer("Pronama_chanIDE2013")

        OnSizeChange()
    End Sub

    Private Sub OnSizeChange() Handles _view.ViewportHeightChanged, _view.ViewportWidthChanged

        'clear the adornment layer of previous adornments
        _adornmentLayer.RemoveAllAdornments()

        'Place the image in the top right hand corner of the Viewport
        'Canvas.SetLeft(Shell, _view.ViewportRight - 60)
        'Canvas.SetTop(Shell, _view.ViewportTop + 30)

        Dim w = Shell.ActualWidth
        Dim h = Shell.ActualHeight

        If Shell.ActualWidth = 0 Then
            w = ShellWidth * (_view.ZoomLevel / 100)
            h = ShellHeight * (_view.ZoomLevel / 100)
        End If

        Canvas.SetLeft(Shell, (_view.ViewportRight - w - 10))
        Canvas.SetTop(Shell, (_view.ViewportBottom - h - 10))

        'add the image to the adornment layer and make it relative to the viewport
        _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, Nothing, Nothing, Shell, Nothing)

    End Sub

End Class
