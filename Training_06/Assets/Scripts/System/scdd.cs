using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class scdd : MonoBehaviour
{
    public TextAsset questionsData;
    private List<Question> questions1;
    private List<Question> questions2;
    private List<Question> questions3;
    public Text questionsJV;
    public Text questionsNS;
    public Text questionsHG;
    public Text questionsMU;
    public Text questionsCS;
    public Text questionsSL;
    public Text questions1JV;
    public Text questions1NS;
    public Text questions1HG;
    public Text questions1MU;
    public Text questions1CS;
    public Text questions1SL;
    public Text questions2JV;
    public Text questions2NS;
    public Text questions2HG;
    public Text questions2MU;
    public Text questions2CS;
    public Text questions2SL;
    public Text questions3JV;
    public Text questions3NS;
    public Text questions3HG;
    public Text questions3MU;
    public Text questions3CS;
    public Text questions3SL;
    public Text totalquestions;
    public Text totalquestions1;
    public Text totalquestions2;
    public Text totalquestions3;

    public class Question
    {
        public int difficulty;
        public string theme;
        public string intitule;
        public string[] reponses = new string[4];
    }
    private void Awake()
    {
        ConvertCSV();
    }
    void ConvertCSV()
    {
        questions1 = new List<Question>();
        questions2 = new List<Question>();
        questions3 = new List<Question>();
        string[] data = questionsData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int questionsNumber = data.Length / 7;
        for (int i = 0; i < questionsNumber; i++)
        {
            switch (int.Parse(data[7 * i]))
            {
                case 1:
                    questions1.Add(new Question());
                    questions1[questions1.Count - 1].theme = data[7 * i + 1];
                    questions1[questions1.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {

                        questions1[questions1.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                case 2:
                    questions2.Add(new Question());
                    questions2[questions2.Count - 1].theme = data[7 * i + 1];
                    questions2[questions2.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        questions2[questions2.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                case 3:
                    questions3.Add(new Question());
                    questions3[questions3.Count - 1].theme = data[7 * i + 1];
                    questions3[questions3.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        questions3[questions3.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                default:
                    break;
            }
            
        }
        int _questions1JV = new int();
        int _questions1NS = new int();
        int _questions1HG = new int();
        int _questions1MU = new int();
        int _questions1CS = new int();
        int _questions1SL = new int();
        for (int i = 0; i < questions1.Count; i++)
        {
            if (questions1[i].theme == "JV")
            {
                _questions1JV ++;
            }
            if (questions1[i].theme == "NS")
            {
                _questions1NS ++;
            }
            if (questions1[i].theme == "HG")
            {
                _questions1HG++;
            }
            if (questions1[i].theme == "MU")
            {
                _questions1MU++;
            }
            if (questions1[i].theme == "CS")
            {
                _questions1CS++;
            }
            if (questions1[i].theme == "SL")
            {
                _questions1SL++;
            }
        }
        questions1JV.text = _questions1JV.ToString();
        questions1NS.text = _questions1NS.ToString();
        questions1HG.text = _questions1HG.ToString();
        questions1MU.text = _questions1MU.ToString();
        questions1CS.text = _questions1CS.ToString();
        questions1SL.text = _questions1SL.ToString();
        int _questions2JV = new int();
        int _questions2NS = new int();
        int _questions2HG = new int();
        int _questions2MU = new int();
        int _questions2CS = new int();
        int _questions2SL = new int();
        for (int i = 0; i < questions2.Count; i++)
        {
            if (questions2[i].theme == "JV")
            {
                _questions2JV ++;
            }
            if (questions2[i].theme == "NS")
            {
                _questions2NS ++;
            }
            if (questions2[i].theme == "HG")
            {
                _questions2HG++;
            }
            if (questions2[i].theme == "MU")
            {
                _questions2MU++;
            }
            if (questions2[i].theme == "CS")
            {
                _questions2CS++;
            }
            if (questions2[i].theme == "SL")
            {
                _questions2SL++;
            }
        }
        questions2JV.text = _questions2JV.ToString();
        questions2NS.text = _questions2NS.ToString();
        questions2HG.text = _questions2HG.ToString();
        questions2MU.text = _questions2MU.ToString();
        questions2CS.text = _questions2CS.ToString();
        questions2SL.text = _questions2SL.ToString();
        int _questions3JV = new int();
        int _questions3NS = new int();
        int _questions3HG = new int();
        int _questions3MU = new int();
        int _questions3CS = new int();
        int _questions3SL = new int();
        for (int i = 0; i < questions3.Count; i++)
        {
            if (questions3[i].theme == "JV")
            {
                _questions3JV++;
            }
            if (questions3[i].theme == "NS")
            {
                _questions3NS++;
            }
            if (questions3[i].theme == "HG")
            {
                _questions3HG++;
            }
            if (questions3[i].theme == "MU")
            {
                _questions3MU++;
            }
            if (questions3[i].theme == "CS")
            {
                _questions3CS++;
            }
            if (questions3[i].theme == "SL")
            {
                _questions3SL++;
            }
        }
        questions3JV.text = _questions3JV.ToString();
        questions3NS.text = _questions3NS.ToString();
        questions3HG.text = _questions3HG.ToString();
        questions3MU.text = _questions3MU.ToString();
        questions3CS.text = _questions3CS.ToString();
        questions3SL.text = _questions3SL.ToString();

        questionsJV.text = (_questions1JV + _questions2JV + _questions3JV).ToString();
        questionsNS.text = (_questions1NS + _questions2NS + _questions3NS).ToString();
        questionsHG.text = (_questions1HG + _questions2HG + _questions3HG).ToString();
        questionsMU.text = (_questions1MU + _questions2MU + _questions3MU).ToString();
        questionsCS.text = (_questions1CS + _questions2CS + _questions3CS).ToString();
        questionsSL.text = (_questions1SL + _questions2SL + _questions3SL).ToString();

        totalquestions1.text = questions1.Count.ToString();
        totalquestions2.text = questions2.Count.ToString();
        totalquestions3.text = questions3.Count.ToString();
        totalquestions.text = (questions1.Count + questions2.Count + questions3.Count).ToString();
    }
}
