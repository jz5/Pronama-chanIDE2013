Imports System.Windows.Media.Imaging
Imports System.Threading.Tasks
Imports System.Threading
Imports System.Windows.Threading

Public Class Shell

    Private Enum Face
        Blink1
        Blink2
        Blink3
        Cry
        Fine
        Proud
        Surprise
        Wink
    End Enum

    Private WithEvents FaceTimer As New DispatcherTimer
    Private Random As New Random
    Private Images As New Dictionary(Of Face, BitmapImage)

    Private Sub RestartTimer()
        FaceTimer.Interval = TimeSpan.FromSeconds(Me.Random.Next(2, 30))
        FaceTimer.Start()
    End Sub


    Sub New()
        InitializeComponent()

        Images.Add(Face.Blink1, New BitmapImage(New Uri("Images/blink1.png", UriKind.Relative)))
        Images.Add(Face.Blink2, New BitmapImage(New Uri("Images/blink2.png", UriKind.Relative)))
        Images.Add(Face.Blink3, New BitmapImage(New Uri("Images/blink3.png", UriKind.Relative)))

        Images.Add(Face.Cry, New BitmapImage(New Uri("Images/cry.png", UriKind.Relative)))
        Images.Add(Face.Fine, New BitmapImage(New Uri("Images/fine.png", UriKind.Relative)))
        Images.Add(Face.Proud, New BitmapImage(New Uri("Images/proud.png", UriKind.Relative)))
        Images.Add(Face.Surprise, New BitmapImage(New Uri("Images/surprise.png", UriKind.Relative)))
        Images.Add(Face.Wink, New BitmapImage(New Uri("Images/wink.png", UriKind.Relative)))

        RestartTimer()
    End Sub

    Public Async Function Fine() As Task
        Me.FaceImage.Source = Images(Face.Fine)
        Await Task.Factory.StartNew(
            Sub()
                Thread.Sleep(TimeSpan.FromSeconds(3))
            End Sub)
    End Function

    Public Async Function Blink() As Task

        Me.FaceImage.Source = Images(Face.Blink2)
        Await Task.Factory.StartNew(
            Sub()
                Thread.Sleep(New TimeSpan(800000))
            End Sub).ContinueWith(
            Sub()
                Me.FaceImage.Source = Images(Face.Blink3)
            End Sub, TaskScheduler.FromCurrentSynchronizationContext).ContinueWith(
            Sub()
                Thread.Sleep(New TimeSpan(800000))

            End Sub).ContinueWith(
            Sub()
                Me.FaceImage.Source = Images(Face.Blink1)
            End Sub, TaskScheduler.FromCurrentSynchronizationContext)

    End Function

    Private Async Sub FaceTimer_Tick(sender As Object, e As EventArgs) Handles FaceTimer.Tick

        FaceTimer.Stop()
        If Random.Next(0, 10) < 8 Then
            Await Blink()
        Else
            FaceImage.Source = Images(Face.Proud)
        End If
        RestartTimer()

    End Sub

    Sub ChangeFace(buildSucess As Boolean)
        If Not FaceTimer.IsEnabled Then ' blinking
            Exit Sub
        End If

        FaceTimer.Stop()
        If buildSucess Then
            If Random.Next(0, 10) < 5 Then
                FaceImage.Source = Images(Face.Fine)
            Else
                FaceImage.Source = Images(Face.Wink)
            End If
        Else
            If Random.Next(0, 10) < 7 Then
                FaceImage.Source = Images(Face.Surprise)
            Else
                FaceImage.Source = Images(Face.Cry)
            End If
        End If
        RestartTimer()
    End Sub


End Class
