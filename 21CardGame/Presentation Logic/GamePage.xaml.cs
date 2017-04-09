﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalCardGame;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _21CardGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private CardGame _game;

        private int _player1Score;
        private int _player2Score;
        private int _player3Score;
        private int _player4Score;

        private int _playerOneScore;
        private int _playerTwoScore;
        private int _playerThreeScore;
        private int _playerFourScore;

        public GamePage()
        {
            this.InitializeComponent();
            
            // Hide each of the players cards
            _cardPlayer1.Opacity = 0.0;
            _cardPlayer2.Opacity = 0.0;
            _cardPlayer3.Opacity = 0.0;
            _cardPlayer4.Opacity = 0.0;

            //initialize the _game field variable
            _game = new CardGame();

            _player1Score = 0;
            _player2Score = 0;
            _player3Score = 0;
            _player4Score = 0;

            _playerOneScore = 0;
            _playerTwoScore = 0;
            _playerThreeScore = 0;
            _playerFourScore = 0;
        }
        private void OnDealCards(object sender, RoutedEventArgs e)
        {
            DealCardsAnimation();

            // Deal the cards
            _game.DealCards();

            // Clear hint
            _txtHint.Text = "";

            // Disable the Deal Cards button
            _btnDealCards.IsEnabled = false;

            // Enable the Flip Cards button
            _btnFlipCards.IsEnabled = true;
        }

        private void OnFlipCards(object sender, RoutedEventArgs e)
        {
            // Restock the deck
            _card1.Visibility = Visibility.Visible;
            _card2.Visibility = Visibility.Visible;
            _card3.Visibility = Visibility.Visible;
            _card4.Visibility = Visibility.Visible;

            //show the cards with one second between each to give the illusion like they are being flipped
            FlipCardsAnimation();

            // Disable Flip Cards button
            _btnFlipCards.IsEnabled = false;
            
            // Enable the Deal Cards button
            _btnDealCards.IsEnabled = true;

            //update the score
            if (_game.PlayRound() == 1)
            {
                _player1Score++;
                _player1Point.Text = _player1Score.ToString();
                if(_player1Score == 5)
                {
                    _txtHint.Text = "Player 1 Won!";
                    _playerOneScore++;
                    _player1LeaderboardScore.Text = _playerOneScore.ToString();
                    ClearResults();
                }
            }

            else if (_game.PlayRound() == 2)
            {
                _player2Score++;
                _player2Point.Text = _player2Score.ToString();
                if (_player2Score == 5)
                {
                    _txtHint.Text = "Player 2 Won!";
                    _playerTwoScore++;
                    _player2LeaderboardScore.Text = _playerTwoScore.ToString();
                    ClearResults();
                }
            }

            else if (_game.PlayRound() == 3)
            {
                _player3Score++;
                _player3Point.Text = _player3Score.ToString();
                if (_player3Score == 5)
                {
                    _txtHint.Text = "Player 3 Won!";
                    _playerThreeScore++;
                    _player3LeaderboardScore.Text = _playerThreeScore.ToString();
                    ClearResults();
                }
            }

            else if (_game.PlayRound() == 4)
            {
                _player4Score++;
                _player4Point.Text = _player4Score.ToString();
                if (_player4Score == 5)
                {
                    _txtHint.Text = "Player 4 Won!";
                    _playerFourScore++;
                    _player4LeaderboardScore.Text = _playerFourScore.ToString();
                    ClearResults();
                }
            }

            else if(_game.PlayRound()== 0)
            {
                
            }

        }

        private void ShowCard(Image imageCtrl, Card card)
        {
            //DONE: determine the name of hte image based on card value and suit
            char suitId = card.Suit.ToString()[0];
            string cardValueId = card.Value.ToString("00");
            string cardImageFileName = $"{suitId}{cardValueId}.png";

            //DONE: display the image in the image control
            string cardImgPath = $"ms-appx:///Assets/Card Assets/{cardImageFileName}";
            imageCtrl.Source = new BitmapImage(new Uri(cardImgPath));

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainPage._player1Name == "")
            {
                _player1Name.Text = "Player";
                _player1LeaderboardName.Text = "Player";
            }
            else
            {
                _player1Name.Text = MainPage._player1Name;
                _player1LeaderboardName.Text = MainPage._player1Name;
            }

            if (MainPage._player2Name == "")
            {
                _player2Name.Text = "CPU 2";
                _player2LeaderboardName.Text = "CPU 2";
            }
            else
            {
                _player2Name.Text = MainPage._player2Name;
                _player2LeaderboardName.Text = MainPage._player2Name;

            }

            if (MainPage._player3Name == "")
            {
                _player3Name.Text = "CPU 3";
                _player3LeaderboardName.Text = "CPU 3";
            }
            else
            {
                _player3Name.Text = MainPage._player3Name;
                _player3LeaderboardName.Text = MainPage._player3Name;
            }

            if (MainPage._player4Name == "")
            {
                _player4Name.Text = "CPU 4";
                _player4LeaderboardName.Text = "CPU 4";
            }
            else
            {
                _player4Name.Text = MainPage._player4Name;
                _player4LeaderboardName.Text = MainPage._player4Name;
            }

        }
        private void ClearResults()
        {
            _player1Score = 0;
            _player2Score = 0;
            _player3Score = 0;
            _player4Score = 0;

            _player1Point.Text = "0";
            _player2Point.Text = "0";
            _player3Point.Text = "0";
            _player4Point.Text = "0";
        }

        private async void OnViewRules(object sender, RoutedEventArgs e)
        {
            // Store the text from instructions.txt in a variable
            string rules = File.ReadAllText("Assets/rules.txt");

            // Display the instructions in a MessageDialog
            var dialog = new MessageDialog(rules, "Game Rules");
            await dialog.ShowAsync();
        }

        private async void OnNewGame(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Are you sure wish to start a new game?  This will reset the current game!");

            dialog.Commands.Add(new UICommand("Yes"));
            dialog.Commands.Add(new UICommand("Cancel"));

            var result = await dialog.ShowAsync();

            if (result.Label == "Yes")
            {
                // Navigates to the Game Page
                this.Frame.Navigate(typeof(MainPage));
            }
        }
        private async void FlipCardsAnimation()
        {
            // Flip the cards

            // Wait for 1 second
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            
            // Flip the first card
            ShowCard(_cardPlayer1, _game.Player1Card);

            // Flip the third card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            ShowCard(_cardPlayer3, _game.Player3Card);

            // Flip the second card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            ShowCard(_cardPlayer2, _game.Player2Card);

            // Flip the fourth card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            ShowCard(_cardPlayer4, _game.Player4Card);
        }

        private async void DealCardsAnimation()
        {
            // Get the path for the back of the card
            string cardImgPath = $"ms-appx:///Assets/Card Assets/playing-card-back.jpg";

            // Remove the cards from the deck and give them to the players

            // Wait for one second
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));

            // Hide the card at the top of the deck
            _card4.Visibility = Visibility.Collapsed;
            
            // Display Player 1s Card
            _cardPlayer1.Source = new BitmapImage(new Uri(cardImgPath));
            _cardPlayer1.Opacity = 1.0;

            // Display Player 3s Card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            _card3.Visibility = Visibility.Collapsed;
            _cardPlayer3.Source = new BitmapImage(new Uri(cardImgPath));
            _cardPlayer3.Opacity = 1.0;

            // Display Player 2s Card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            _card2.Visibility = Visibility.Collapsed;
            _cardPlayer2.Source = new BitmapImage(new Uri(cardImgPath));
            _cardPlayer2.Opacity = 1.0;

            // Display Player 4s Card
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
            _card1.Visibility = Visibility.Collapsed;
            _cardPlayer4.Source = new BitmapImage(new Uri(cardImgPath));
            _cardPlayer4.Opacity = 1.0;
        }
    }
}
