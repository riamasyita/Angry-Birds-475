using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private TrailController trailController;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _statusInfo;
    public SlingShooter SlingShooter;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    private Bird _shotBird;
    public BoxCollider2D TapCollider;
    public Menu _menu;

    private bool _isGameEnded = false;

    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    private void OnDestroy()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed -= ChangeBird;
            Birds[i].OnBirdShot -= AssignTrail;
        }
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded || Birds.Count <= 0) return;

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
    }

    public void AssignTrail(Bird bird)
    {
        TapCollider.enabled = true;
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
            _menu.scene_2();
        }
        if (sceneName == "belajar-AngryBirds 1" && Enemies.Count == 0)
        {
            _statusInfo.text = "You Win!";
            _panel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}
