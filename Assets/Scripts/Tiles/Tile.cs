using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] gemContainers;

    private void OnEnable()
    {
        foreach(var container in gemContainers)
        {
            var i = Random.Range(0, 3);
            if (i > 0)
                container.SetActive(false);
            else
            {
                container.SetActive(true);
                foreach (Transform gem in container.transform) gem.gameObject.SetActive(true);
            }
        }
    }
}
