using System;
using Game;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [NonSerialized] public int score;
        [NonSerialized] public int buff;
        [NonSerialized] public int debuff;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI buffText;
        public TextMeshProUGUI debuffText;
        public TextMeshProUGUI totalBuffText;
        public GameObject[] characterPrefabs;
        public bool isPlayer1;
        [NonSerialized] public GameObject player;

        private void Awake()
        {
            var playerName = isPlayer1 ? "Player1" : "Player2";
            var index = isPlayer1 ? JsonManager.GetSelectedCharacterPlayer1() : JsonManager.GetSelectedCharacterPlayer2();
            var position = transform.position + (isPlayer1 ? new Vector3(-25, 0, 0) : new Vector3(25, 0, 0));
            player = Instantiate(characterPrefabs[index], position, Quaternion.identity);
            player.name = playerName;
            player.GetComponent<PlayerController>().isPlayer1 = isPlayer1;
        }

        void Start() { score = 0; }

        void Update()
        {
            scoreText.text = score.ToString();
            buffText.text = buff.ToString();
            debuffText.text = debuff.ToString();
            totalBuffText.text = ((buff - debuff) / 10f + 1).ToString("0.0") + "X";
        }
    }
}