Public Class PaquetDeCartes

    ' listes des figure de chaque couleur
    Private CardListCoeur As New List(Of String)({"deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "valet", "dame", "roi", "as"})
    Private CardListPique As New List(Of String)({"deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "valet", "dame", "roi", "as"})
    Private CardListCarreau As New List(Of String)({"deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "valet", "dame", "roi", "as"})
    Private CardListTrefle As New List(Of String)({"deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "valet", "dame", "roi", "as"})

    ' listes des figure de chaque couleur shuffled
    Public shuffledHearths As List(Of String)
    Public shuffledSpades As List(Of String)
    Public shuffledDiamonds As List(Of String)
    Public shuffledClubs As List(Of String)

    ' compteur de figure de chaque couleur
    Public cptCoeur As Integer = 0
    Public cptPique As Integer = 0
    Public cptCarreau As Integer = 0
    Public cptTrefle As Integer = 0


    Public Sub New()
        ' CONSTRUCTEUR
        Me.shuffledHearths = Shuffle(CardListCoeur)
        Me.shuffledSpades = Shuffle(CardListCoeur)
        Me.shuffledDiamonds = Shuffle(CardListCoeur)
        Me.shuffledClubs = Shuffle(CardListCoeur)

    End Sub

    ' reset le compteur et etant donner on a passer  travers le paquet, on le re-shuffle
    Public Sub cptReset()
        If cptCoeur = 13 And cptPique = 13 And cptCarreau = 13 And cptTrefle = 13 Then
            cptCoeur = 0 And cptPique = 0 And cptCarreau = 0 And cptTrefle = 0
        End If
        ReShuffle()
    End Sub

    ' on shuffle avec une methode qui utilise du LinkQ (trouver sur internet, je la comprend pas a 100% mais pas pire)
    Function Shuffle(Of T)(collection As IEnumerable(Of T)) As List(Of T)
        Dim rnd As New Random
        Shuffle = collection.OrderBy(Function(a) rnd.Next()).ToList()
    End Function

    Public Sub ReShuffle()
        Me.shuffledHearths = Shuffle(shuffledHearths)
        Me.shuffledSpades = Shuffle(shuffledSpades)
        Me.shuffledDiamonds = Shuffle(shuffledDiamonds)
        Me.shuffledClubs = Shuffle(shuffledClubs)
    End Sub

End Class