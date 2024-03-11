using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{

    [SerializeField]  Transform capybaraPrefab;
    [SerializeField]  Transform capybaraHolder;
    [SerializeField] private TMPro.TextMeshProUGUI vidasText;

    private Transform currentCapybara = null;
    private Rigidbody2D currentRigidbody;

    //El lugar de inicio de los capibaras.
    private Vector2 capybaraStartPosition = new Vector2(0f, 4f);

    private float capybaraSpeed = 6f;
    private float capybaraSpeedIncrement = 0.5f;
    private int capybaraDirection = 1;
    private float xLimit = 5;

    private float timeBetweenRounds = 1f;

    private int startingLives = 3;
    private int livesRemaining;
    private bool playing = true;

    void Start()
    {
        livesRemaining = startingLives;
        vidasText.text = $"{livesRemaining}";
        SpawnNewCapybara();
    }

    //Sirve para crear un objeto de acuerdo a las caracteristicas deseadas.
    private void SpawnNewCapybara()
    {
        currentCapybara = Instantiate(capybaraPrefab, capybaraHolder);
        currentCapybara.position = capybaraStartPosition;

        currentRigidbody = currentCapybara.GetComponent<Rigidbody2D>(); 

        //Aumento de dificultad.
        capybaraSpeed += capybaraSpeedIncrement;

    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
        SpawnNewCapybara();
    }

   //Añadir altura al momento de stackear a los capibaras. NO ME SIRVIÓ, AAAAA, TRISTE
   // private void RaiseCapybara()
   //{
   //     currentCapybara.transform.position = new Vector3(currentCapybara.transform.position.x, currentCapybara.transform.position.y + stackHeight, currentCapybara.transform.position.z);
   //}

    void Update()
    {
        //Crear un limite donde se spawnean los capibaras, cuando se llega al limite marcado, entonces se regresa, como si estuviera rebotando.
        if (currentCapybara && playing)
        {
            float moveAmount = Time.deltaTime * capybaraSpeed * capybaraDirection;
            currentCapybara.position += new Vector3(moveAmount, 0, 0);
        if (Mathf.Abs(currentCapybara.position.x) > xLimit)  
        {
            currentCapybara.position = new Vector3(capybaraDirection * xLimit, currentCapybara.position.y, 0);
            capybaraDirection = -capybaraDirection;
        }
        
        //Tecla para poder tirar al capibara que se desea.

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCapybara = null;
            currentRigidbody.simulated = true; //Sirve para activar la gravedad al momento de presionar la tecla, ocasionando que caiga.
           StartCoroutine(DelayedSpawn());
        }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
     }

     public void RemoveLife()
     {
         livesRemaining = Mathf.Max(livesRemaining - 1, 0);
         vidasText.text = $"{livesRemaining}";

         if(livesRemaining == 0)
           { 
               playing = false;
           }
     }
}



//https://www.youtube.com/watch?v=VqHaDUXJsyg