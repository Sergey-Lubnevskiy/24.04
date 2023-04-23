using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {
        private List<Question> questions; // Список вопросов
        private int currentQuestionIndex; // Индекс текущего вопроса
        private int correctAnswersCount; // Количество правильных ответов
        private int money; // Количество заработанных денег

        public Form1()
        {
            InitializeComponent();
            LoadQuestions(); // Загрузка вопросов из текстового файла
            currentQuestionIndex = 0; // Установка текущего вопроса на первый
            correctAnswersCount = 0;
            money = 0;
            ShowQuestion(); // Отображение первого вопроса на форме
        }

        private void LoadQuestions()
        {
            questions = new List<Question>();
            StreamReader sr = new StreamReader("question.txt");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(',');
                string text = parts[0];
                string[] answers = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    answers[i] = parts[i + 1];
                }
                questions.Add(new Question(text, answers));
            }
            sr.Close();
        }

        private void ShowQuestion()
        {
            Question question = questions[currentQuestionIndex];
            questionLabel.Text = question.Text;
            answerButton1.Text = question.Answers[0];
            answerButton2.Text = question.Answers[1];
            answerButton3.Text = question.Answers[2];
            answerButton4.Text = question.Answers[3];
        }

        private void CheckAnswer(int answerIndex)
        {
            SoundPlayer True = new SoundPlayer(@"\\true.wav");
            SoundPlayer False = new SoundPlayer(@"\\false.wav");
            Question question = questions[currentQuestionIndex];
            if (question.CorrectAnswerIndex == answerIndex)
            {
                correctAnswersCount++;
                money += question.Prize;
                if (currentQuestionIndex < questions.Count - 1)
                {
                    currentQuestionIndex++;
                    ShowQuestion();
                }
                else
                {
                    True.Play();
                    MessageBox.Show("Вывыиграли " + money.ToString() + " долларов!");
                    this.Close();
                }
            }
            else
            {
                False.Play();
                MessageBox.Show("Вы ответили неправильно! Конец игры!");

                this.Close();
            }
        }
        private void answerButton1_Click(object sender, EventArgs e)
        {
            CheckAnswer(0);
        }

        private void answerButton2_Click(object sender, EventArgs e)
        {
            CheckAnswer(1);
        }

        private void answerButton3_Click(object sender, EventArgs e)
        {
            CheckAnswer(2);
        }

        private void answerButton4_Click(object sender, EventArgs e)
        {
            CheckAnswer(3);
        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            SoundPlayer Winner = new SoundPlayer(@"\\winner.wav");
            Winner.Play();
            MessageBox.Show("Вы ответили на " + correctAnswersCount.ToString() + " вопросов из " + questions.Count.ToString() + " и выиграли " + money.ToString() + " долларов!");
            this.Close();
        }
    }

    public class Question
    {
        public string Text { get; set; } // Текст вопроса
        public string[] Answers { get; set; } // Варианты ответов
        public int CorrectAnswerIndex { get; set; } // Индекс правильного ответа
        public int Prize { get; set; } // Сумма денег, которую можно выиграть
        public Question(string text, string[] answers)
        {
            Text = text;
            Answers = answers;
            CorrectAnswerIndex = 0; // Изначально правильный ответ - первый
            Prize = 0; // Изначально сумма выигрыша - 0
        }

    }
}

