Imports System.Media
Imports System.IO
Imports System.Windows

Public Class Form1
    ' Instancier le paquet de carte
    Private joueur As New PaquetDeCartes

    ' listes des couleur et figure possible
    Private figureList As New List(Of String)({"deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "valet", "dame", "roi", "as"})
    Private couleurList As New List(Of String)({"coeur", "pique", "carreau", "trefle"})

    ' listes des couleur et figure du joueur
    Private figureDuJoueur As List(Of String)
    Private couleurDuJoueur As List(Of String)

    ' listes de verifications
    Private listPair As New List(Of String)
    Private listPairInverser As New List(Of String)
    Private listThreeOfAkind As New List(Of String)

    Private PlayOrStop As Boolean = False



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PlayLoopingBackgroundSoundResource()    ' JOUE LA MUSIQUE AU LOAD

        BtnAjouterCredit.Enabled = False
        BtnContinuer.Enabled = False
        BtnMiser.Enabled = False
        BtnCommencer.Enabled = True
        BtnQuitter.Enabled = True

        NumCredit.Text = 100     ' ON DONNE 100 CREDITS DE DEPART
    End Sub

    '   **********  METHODE POUR JOUER LA MUSIQUE   **********
    Sub PlayLoopingBackgroundSoundResource()
        Dim path As String = Directory.GetCurrentDirectory()
        Dim pathParent1 As DirectoryInfo = Directory.GetParent(path)
        Dim pathParent2 As DirectoryInfo = Directory.GetParent(pathParent1.ToString())
        Dim pathParent3 As DirectoryInfo = Directory.GetParent(pathParent2.ToString())
        Dim truePath As String = pathParent3.ToString() & "\Resources\pokerFace.wav"
        My.Computer.Audio.Play(truePath, AudioPlayMode.BackgroundLoop)
    End Sub

    '   ********** ON RESET LES CHECKBOX AVANT LA PROCHAINE PARTIE  **********
    Private Sub ResetCheckBox()
        ChkKeep1.Checked = False
        ChkKeep2.Checked = False
        ChkKeep3.Checked = False
        ChkKeep4.Checked = False
        ChkKeep5.Checked = False
    End Sub

    '   **********  ON AJOUTE LA MAIN DU JOUEUR DANS DEUX LISTES (couleur et figure)  **********
    Function LesFiguresDuJoueur()
        Dim list As New List(Of String)
        list.Add(LblNumber1.Text)
        list.Add(LblNumber2.Text)
        list.Add(LblNumber3.Text)
        list.Add(LblNumber4.Text)
        list.Add(LblNumber5.Text)
        Return list
    End Function

    Function LesCouleurDuJoueur()
        Dim list As New List(Of String)
        list.Add(LblColor1.Text)
        list.Add(LblColor2.Text)
        list.Add(LblColor3.Text)
        list.Add(LblColor4.Text)
        list.Add(LblColor5.Text)
        Return list
    End Function

    '   **********  METHODE POUR ATTRIBUER UNE CARTE    **********
    Private Sub AttribuerCarte(carteCouleur As Label, carteFigure As Label)
        Dim color As Integer
        Do
            Dim rnd As New Random
            color = rnd.Next(1, 4)
            Select Case color
                Case 1
                    If joueur.cptCoeur = 13 Then
                        Continue Do             ' LAST CHANGE
                    End If
                    carteCouleur.Text = "Coeur"
                    carteFigure.Text = joueur.shuffledHearths(joueur.cptCoeur)
                    joueur.cptCoeur += 1
                Case 2
                    If joueur.cptPique = 13 Then
                        Continue Do
                    End If
                    carteCouleur.Text = "Pique"
                    carteFigure.Text = joueur.shuffledSpades(joueur.cptPique)
                    joueur.cptPique += 1
                Case 3
                    If joueur.cptCarreau = 13 Then
                        Continue Do
                    End If
                    carteCouleur.Text = "Carreau"
                    carteFigure.Text = joueur.shuffledDiamonds(joueur.cptCarreau)
                    joueur.cptCarreau += 1
                Case 4
                    If joueur.cptTrefle = 13 Then
                        Continue Do
                    End If
                    carteCouleur.Text = "Trefle"
                    carteFigure.Text = joueur.shuffledClubs(joueur.cptTrefle)
                    joueur.cptTrefle += 1
            End Select
        Loop While carteCouleur.Text = "" And carteFigure.Text = ""
    End Sub

    '   **********  METHODE POUR RE-ATTRIBUER UN NOMBRE DE CARTE    **********
    Private Sub ReAttributionDesCartes()
        Do
            If ChkKeep1.Checked = False Then
                AttribuerCarte(LblColor1, LblNumber1)
            End If

            If ChkKeep2.Checked = False Then
                AttribuerCarte(LblColor2, LblNumber2)
            End If

            If ChkKeep3.Checked = False Then
                AttribuerCarte(LblColor3, LblNumber3)
            End If

            If ChkKeep4.Checked = False Then
                AttribuerCarte(LblColor4, LblNumber4)
            End If

            If ChkKeep5.Checked = False Then
                AttribuerCarte(LblColor5, LblNumber5)
            End If
        Loop While NumMise.Text <= 0
    End Sub


    '   **********  METHODE POUR VERIFIER SI LE JOUEUR A UNE MAIN GAGNANTE  **********
    Private Sub Verification()
        If RoyalFlush() = True Then
            Dim gain As Integer = NumMise.Value * 100
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Royal Flush a total of " & gain & " jetons"

        ElseIf StraightFlush() = True Then
            Dim gain As Integer = NumMise.Value * 50
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Straight Flush a total of " & gain & " jetons"

        ElseIf FourOfAKind() = True Then
            Dim gain As Integer = NumMise.Value * 25
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Four Of A Kind a total of " & gain & " jetons"

        ElseIf FullHouse() = True Then
            Dim gain As Integer = NumMise.Value * 20
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Full House a total of " & gain & " jetons"

        ElseIf Flush() = True Then
            Dim gain As Integer = NumMise.Value * 15
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Flush a total of " & gain & " jetons"

        ElseIf Straight() = True Then
            Dim gain As Integer = NumMise.Value * 10
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Straight a total of " & gain & " jetons"

        ElseIf ThreeOfAKind() = True Then
            Dim gain As Integer = NumMise.Value * 5
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Three Of A Kind a total of " & gain & " jetons"

        ElseIf TwoPairs() = True And FullHouse() = False Then
            Dim gain As Integer = NumMise.Value * 3
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Two Pairs a total of " & gain & " jetons"

        ElseIf Pair() = True Then
            Dim gain As Integer = NumMise.Value * 1
            NumGains.Value = NumGains.Value + gain
            TextBox1.Text = "You Win with a Pair a total of " & gain & " jetons"

        Else
            TextBox1.Text = "You Loose - Dont give up, and Keep Playing !"
        End If

    End Sub


    '   **********  CALCUL DES MAINS GAGNANTE  **********
    Function RoyalFlush()
        Dim isSameColor As Boolean = SameColor()
        If isSameColor = True And figureDuJoueur.Contains("as") And figureDuJoueur.Contains("roi") And figureDuJoueur.Contains("dame") And figureDuJoueur.Contains("valet") And figureDuJoueur.Contains("dix") Then
            Return True
        Else
            Return False
        End If
    End Function

    Function StraightFlush()
        Dim isSameColor As Boolean = SameColor()
        Dim isSuite As Boolean = Suite()
        If isSameColor = True And isSuite = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Function FourOfAKind()
        Dim i As Integer
        Dim j As Integer
        Dim list As New List(Of String)
        For i = 0 To 4 Step 1
            list.Clear()
            For j = 0 To 4 Step 1
                If figureDuJoueur(i) = figureDuJoueur(j) Then
                    list.Add(figureDuJoueur(j))
                    If list.Count - 1 = 3 Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function

    Function FullHouse()
        If ThreeOfAKind() = True And PairInverser() = True And listThreeOfAkind(0) <> listPairInverser(0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Function Flush()
        Dim isSameColor As Boolean = SameColor()
        If isSameColor = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Function Straight()
        Dim isSuite As Boolean = Suite()
        If isSuite = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Function ThreeOfAKind()
        Dim i As Integer
        Dim j As Integer
        For i = 0 To 4 Step 1
            listThreeOfAkind.Clear()
            For j = 0 To 4 Step 1
                If figureDuJoueur(i) = figureDuJoueur(j) Then
                    listThreeOfAkind.Add(figureDuJoueur(j))
                    If listThreeOfAkind.Count - 1 = 2 Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function

    Function TwoPairs()
        If Pair() = True And PairInverser() = True And listPair(0) <> listPairInverser(0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Function Pair()
        Dim i As Integer
        Dim j As Integer
        For i = 0 To 4 Step 1
            listPair.Clear()
            For j = 0 To 4 Step 1
                If figureDuJoueur(i) = figureDuJoueur(j) Then
                    listPair.Add(figureDuJoueur(j))
                    If listPair.Count - 1 = 1 Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function



    '   **********  'SOUS-METHODE' DES METHODES DE CALCUL DES MAINS GAGNANTES   **********

    ' comme la pair mais inverser afin de terminer si nous avons deux pairs
    Function PairInverser()
        Dim i As Integer
        Dim j As Integer
        For i = 4 To 0 Step -1
            listPairInverser.Clear()
            For j = 4 To 0 Step -1
                If figureDuJoueur(i) = figureDuJoueur(j) Then
                    listPairInverser.Add(figureDuJoueur(j))
                    If listPairInverser.Count - 1 = 1 Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function

    ' determiner si on a des couleur pareil (flush)
    Function SameColor()
        Dim color As String = couleurDuJoueur(0)
        Dim isSameColor As Boolean = False
        If couleurDuJoueur(1) = color And couleurDuJoueur(2) = color And couleurDuJoueur(3) = color And couleurDuJoueur(4) = color Then
            isSameColor = True
        End If
        Return isSameColor
    End Function

    ' determiner si une suite (straight) est presente
    Function Suite()
        Dim figures As New List(Of Integer)
        figures = ConvertFigures(figureList)
        Dim isSuite As Boolean = False
        If figures.Max - figures.Min = 4 Then
            isSuite = True
        ElseIf figures.Sum = 47 And figures.Contains(1) Then
            isSuite = True
        Else
            isSuite = False
        End If
        Return isSuite
    End Function

    '   Convertir les figures en integer a des fin de calcul
    Function ConvertFigures(figuresList As List(Of String))
        Dim convertedFigures As New List(Of Integer)
        Dim i As Byte
        Dim j As Byte
        For i = 0 To 4 Step 1
            For j = 0 To 12 Step 1
                If figureDuJoueur(i) = figuresList(j) Then
                    convertedFigures.Add(j + 2)
                End If
            Next
        Next
        If convertedFigures.Contains(14) Then
            convertedFigures.Remove(14)
            convertedFigures.Add(1)
        End If
        Return convertedFigures
    End Function


    '   **********  EVENEMENTS  **********

    Private Sub BtnMusique_Click(sender As Object, e As EventArgs) Handles BtnMusique.Click
        Dim path As String = Directory.GetCurrentDirectory()
        Dim pathParent1 As DirectoryInfo = Directory.GetParent(path)
        Dim pathParent2 As DirectoryInfo = Directory.GetParent(pathParent1.ToString())
        Dim pathParent3 As DirectoryInfo = Directory.GetParent(pathParent2.ToString())
        Dim truePath As String = pathParent3.ToString() & "\Resources\pokerFace.wav"

        If PlayOrStop = True Then
            My.Computer.Audio.Play(truePath, AudioPlayMode.BackgroundLoop)
            PlayOrStop = False
        Else
            My.Computer.Audio.Stop()
            PlayOrStop = True
        End If
    End Sub

    Private Sub BtnCommencer_Click(sender As Object, e As EventArgs) Handles BtnCommencer.Click
        AttribuerCarte(LblColor1, LblNumber1)
        AttribuerCarte(LblColor2, LblNumber2)
        AttribuerCarte(LblColor3, LblNumber3)
        AttribuerCarte(LblColor4, LblNumber4)
        AttribuerCarte(LblColor5, LblNumber5)

        BtnCommencer.Enabled = False
        BtnMiser.Enabled = True
        BtnAjouterCredit.Enabled = True
        BtnContinuer.Enabled = False
        BtnQuitter.Enabled = True
        GrpBoard.Enabled = True

        ResetCheckBox()

        NumericUpDown1.Value += 1
        NumMise.Value = 0
        NumMise.BackColor = Color.White
    End Sub

    Private Sub BtnMiser_Click(sender As Object, e As EventArgs) Handles BtnMiser.Click

        BtnContinuer.Enabled = True
        If NumMise.Text = 0 Then
            NumMise.BackColor = Color.DarkRed
        Else
            NumMise.BackColor = Color.MediumSeaGreen
        End If

        Dim mise As Byte = NumMise.Text
        NumCredit.Text = NumCredit.Text - mise
        If NumMise.BackColor = Color.MediumSeaGreen Then
            BtnMiser.Enabled = False
        End If
    End Sub

    Private Sub BtnContinuer_Click(sender As Object, e As EventArgs) Handles BtnContinuer.Click
        ReAttributionDesCartes()

        BtnContinuer.Enabled = False
        GrpBoard.Enabled = False
        BtnMiser.Enabled = False
        BtnAjouterCredit.Enabled = True
        BtnCommencer.Enabled = True

        couleurDuJoueur = LesCouleurDuJoueur()
        figureDuJoueur = LesFiguresDuJoueur()

        Verification()
    End Sub

    Private Sub BtnAjouterCredit_Click(sender As Object, e As EventArgs) Handles BtnAjouterCredit.Click
        NumCredit.Value = NumCredit.Value + NumGains.Value
        NumGains.Value = 0
        BtnAjouterCredit.Enabled = False
    End Sub

    Private Sub BtnQuitter_Click(sender As Object, e As EventArgs) Handles BtnQuitter.Click
        Application.Exit()
    End Sub
End Class