using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject[] cubes; //오브젝트 배열
    private Vector3[] originalPos; //처음위치
    private bool isCube = false;
    private Vector3[] mExArray = new Vector3[] { new Vector3(-32, -1.3f, -8), new Vector3(-14, -1.3f, -8), new Vector3(-32, -1.3f, -13), new Vector3(-14, -1.3f, -13) };
    //이동위치
    public void MoveCubes(QuizManager qm)
    {
        if (cubes != null && !isCube)
        {
            originalPos = new Vector3[cubes.Length]; //처음위치 초기화

            //이동위치 섞기
            for (int i = mExArray.Length - 1; i > 0; i--)
            {
                //int j = qm.rdNext();
                int j = Random.Range(0, i + 1);
                Vector3 temp = mExArray[i];
                mExArray[i] = mExArray[j];
                mExArray[j] = temp;
            }
            // 오브젝트 랜덤 위치 이동, 처음위치 저장
            for (int i = 0; i < cubes.Length; i++)
            {
                Vector3 targetPosition = mExArray[i % mExArray.Length]; // 랜덤한 위치 설정
                originalPos[i] = cubes[i].transform.position; // 원래 위치 저장
                StartCoroutine(CubePosition(cubes[i], targetPosition)); //큐브이동
            }
            isCube = true;
        }
    }

    IEnumerator CubePosition(GameObject cubeObject, Vector3 targetPosition) //큐브이동
    {
        float MoveTime = 0.5f; // 이동하는 시간
        float GoTime = 0f; // 경과한 시간
        Vector3 startPosition = cubeObject.transform.position;

        while (GoTime < MoveTime)
        {
            GoTime += Time.deltaTime; //시간증가
            float t = Mathf.Clamp01(GoTime / MoveTime); //이동거리 비율
            cubeObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t); //이동
            yield return null;
        }
    }

    public void RemoveCube1()
    {
        if (cubes != null && isCube)
        {
            StartCoroutine(CubePosition(cubes[1], originalPos[1]));
            StartCoroutine(CubePosition(cubes[2], originalPos[2]));
            StartCoroutine(CubePosition(cubes[3], originalPos[3]));
            StartCoroutine(EndQuiz4AfterDelay(1f));
        }
        IEnumerator EndQuiz4AfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(CubePosition(cubes[0], originalPos[0]));
        }
        isCube = false;
    }
    public void RemoveCube2()
    {
        if (cubes != null && isCube)
        {
            StartCoroutine(CubePosition(cubes[0], originalPos[0]));
            StartCoroutine(CubePosition(cubes[2], originalPos[2]));
            StartCoroutine(CubePosition(cubes[3], originalPos[3]));
            StartCoroutine(EndQuiz4AfterDelay(1f));
        }
        IEnumerator EndQuiz4AfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(CubePosition(cubes[1], originalPos[1]));
        }
        isCube = false;
    }
    public void RemoveCube3()
    {
        if (cubes != null && isCube)
        {
            StartCoroutine(CubePosition(cubes[0], originalPos[0]));
            StartCoroutine(CubePosition(cubes[1], originalPos[1]));
            StartCoroutine(CubePosition(cubes[3], originalPos[3]));
            StartCoroutine(EndQuiz4AfterDelay(1f));
        }
        IEnumerator EndQuiz4AfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(CubePosition(cubes[2], originalPos[2]));
        }
        isCube = false;
    }
    public void RemoveCube4()
    {
        if (cubes != null && isCube)
        {
            StartCoroutine(CubePosition(cubes[0], originalPos[0]));
            StartCoroutine(CubePosition(cubes[1], originalPos[1]));
            StartCoroutine(CubePosition(cubes[2], originalPos[2]));
            StartCoroutine(EndQuiz4AfterDelay(1f));
        }
        IEnumerator EndQuiz4AfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(CubePosition(cubes[3], originalPos[3]));
        }
        isCube = false;
    }
}
