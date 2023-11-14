using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject[] characterPrefabs;
    public bool isPlayer1;

    private void Awake()
    {
        var playerName = isPlayer1 ? "Player1" : "Player2";
        var index = PlayerPrefs.GetInt("SelectedCharacter" + playerName);
        var position = transform.position + (isPlayer1 ? new Vector3(-25, 0, 0) : new Vector3(25, 0, 0));
        var player = Instantiate(characterPrefabs[index], position, Quaternion.identity);
        player.name = playerName;
        player.GetComponent<PlayerController>().isPlayer1 = isPlayer1;
    }

    void Start() { score = 0; }

    void Update() { scoreText.text = score.ToString(); }
}