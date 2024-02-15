using System.ComponentModel;
using System.Diagnostics;

namespace HangmanAssignment;

public partial class HangmanGamePage : ContentPage, INotifyPropertyChanged
{
    //Variables
    List<char> guessed = new();
    string answer = string.Empty;
    private string _spotlgt;
    private int error = 0;
    private List<char> _Alphabetletters = new();
    private string _Statmessage;
    private string _gameStatus = "Errors : 0 of 8";
    private string _currentImage = "hang1.png";

    #region fields
    public string Spotlgt
    {
        get => _spotlgt;
        set
        {
            _spotlgt = value;
            OnPropertyChanged();
        }
    }

    //The list of words that will generate at random
    List<string> words = new List<string>()
     {
        "arrangement",
        "attempt",
        "border",
        "brick",
        "customs",
        "discussion",
        "essential",
        "exchange",
        "explanation",
        "fireplace",
        "floating",
        "garage",
        "grabbed",
        "grandmother",
        "heading",
        "independent",
        "instant",
        "manufacturing",
        "mathematics",
        "memory",
        "mysterious",
        "neighborhood",
        "occasionally",
        "official",
        "policeman",
        "positive",
        "possibly",
        "practical",
        "promised",
        "remarkable",
        "require",
        "satisfied",
        "scared",
        "selection",
        "shaking",
        "shallow",
        "simplest",
        "slight",
        "slope",
        "species",
        "thumb",
        "tobacco",
        "treated",
        "vessels"
     };

    public string CrtImageName
    {
        get => _currentImage; set
        {
            _currentImage = value;
            OnPropertyChanged();
        }
    }

    public string GameStatus
    {
        get => _gameStatus; set
        {
            _gameStatus = value;
            OnPropertyChanged();
        }
    }

    public string StatusMessage
    {
        get => _Statmessage; set
        {
            _Statmessage = value;
            OnPropertyChanged();
        }
    }

    public List<char> AlphabetLetters
    {
        get => _Alphabetletters;
        set
        {
            _Alphabetletters = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public HangmanGamePage()
	{
		InitializeComponent();
        _Alphabetletters.AddRange("abcdefghijklmnopqrstuvwxyz");
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessed);
    }

    #region gameEngine

    private void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];

        Debug.WriteLine(answer);
    }

    private void CalculateWord(string answer, List<char> guessed)
    {
        var temp = answer.Select(x => guessed.IndexOf(x) >= 0 ? x : '_').ToArray();
        Spotlgt = string.Join(' ', temp);
    }

    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var letter = btn.Text;
            btn.IsEnabled = false;
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        if (guessed.IndexOf(letter) == -1)
        {
            guessed.Add(letter);
        }

        if (answer.IndexOf(letter) >= 0)
        {
            CalculateWord(answer, guessed);
            CheckGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            error++;
            SetGameStatus();
            CheckIfGameLost();
        }
    }

    private void SetGameStatus()
    {
        GameStatus = $"Errors: {error} of {8}";
        SetCurrentImage();

    }

    private void SetCurrentImage()
    {
        CrtImageName = $"hang{error}.png";
    }

    private void CheckIfGameLost()
    {
        if (error == 8)
        {
            StatusMessage = "Game Over...";

            DisableLetters();
        }
    }

    private void DisableLetters()
    {
        foreach (var item in letterContainer.Children)
        {
            var btn = item as Button;

            if (btn != null)
                btn.IsEnabled = false;
        }
    }

    private void EnableLetters()
    {
        foreach (var item in letterContainer.Children)
        {
            var btn = item as Button;

            if (btn != null)
                btn.IsEnabled = true;
        }
    }

    private void CheckGameWon()
    {
        if (answer == Spotlgt.Replace(" ", ""))
        {
            StatusMessage = "You Won!";
            DisableLetters();
        }
    }

    private void TryAgain_Clicked(object sender, EventArgs e)
    {
        error = 0;
        StatusMessage = string.Empty;
        guessed = new();
        PickWord();
        CalculateWord(answer, guessed);
        SetGameStatus();
        EnableLetters();
    }
}