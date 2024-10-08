using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // 게임 점수를 저장하고 private set을 통해 외부에서 직접 변경하지 못하게 함. 초기값은 0.
    //{ get; private set; }에서 get은 프로퍼티의 값을 읽어오며 set은 해당 프로퍼티의 값을 변경할수 있고 
    // get; 앞에는 아무런 제한자가 없어 외부에서 값을 받아올수 있지만  private set;를 통해 내부에서 값을 조정하지 못하게 막을수 있다.
    public int Score { get; private set; } = 0;

    // UI Text 컴포넌트의 참조. 이것은 점수를 화면에 표시하는 데 사용된다.
    public Text scoreText;


    private void OnCollisionEnter(Collision other)
    {
        //만약 Fruit태그가 붙은 오브젝트랑 콜라이더 되면
        if (other.gameObject.CompareTag("Fruit"))
        {
            // 점수를 5점씩 증가
            IncreaseScore(5);
            //디버그 출력
            Debug.Log("5점");
        }
        //만약 FruitHalf태그가 붙은 오브젝트랑 콜라이더 되면
        else if (other.gameObject.CompareTag("FruitHalf"))
        {
            // 점수를 10점씩 증가
            IncreaseScore(10);
            //디버그 출력
            Debug.Log("10점");
        }
        //만약 FruitQaud태그가 붙은 오브젝트랑 콜라이더 되면
        else if (other.gameObject.CompareTag("FruitQaud"))
        {
            //점수를 15점씩 증가
            IncreaseScore(15);
            //디버그 출력
            Debug.Log("15점");
        }
    }
    // 점수를 증가시키는 메소드. increment로 증가시킬 점수 값을 받는다.
    public void IncreaseScore(int increment)
    {
        // 받은 increment만큼 점수를 증가시킨ㄴ다.
        Score += increment;

        // UI Text 컴포넌트의 text 속성을 업데이트하여 변경된 점수를 화면에 표시한다.
        scoreText.text = "Score: " + Score;
    }

}
