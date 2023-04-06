using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject[] cubes; //������Ʈ �迭
    private Vector3[] originalPos; //ó����ġ
    private bool isCube = false;
    private Vector3[] mExArray = new Vector3[] { new Vector3(-32, -1.3f, -8), new Vector3(-14, -1.3f, -8), new Vector3(-32, -1.3f, -13), new Vector3(-14, -1.3f, -13) };
    //�̵���ġ
    public void MoveCubes()
    {
        if (cubes != null && !isCube)
        {
            originalPos = new Vector3[cubes.Length]; //ó����ġ �ʱ�ȭ

            //�̵���ġ ����
            for (int i = mExArray.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Vector3 temp = mExArray[i];
                mExArray[i] = mExArray[j];
                mExArray[j] = temp;
            }
            // ������Ʈ ���� ��ġ �̵�, ó����ġ ����
            for (int i = 0; i < cubes.Length; i++)
            {
                Vector3 targetPosition = mExArray[i % mExArray.Length]; // ������ ��ġ ����
                originalPos[i] = cubes[i].transform.position; // ���� ��ġ ����
                StartCoroutine(CubePosition(cubes[i], targetPosition)); //ť���̵�
            }
            isCube = true;
        }
    }

    IEnumerator CubePosition(GameObject cubeObject, Vector3 targetPosition) //ť���̵�
    {
        float MoveTime = 0.5f; // �̵��ϴ� �ð�
        float GoTime = 0f; // ����� �ð�
        Vector3 startPosition = cubeObject.transform.position;

        while (GoTime < MoveTime)
        {
            GoTime += Time.deltaTime; //�ð�����
            float t = Mathf.Clamp01(GoTime / MoveTime); //�̵��Ÿ� ����
            cubeObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t); //�̵�
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
