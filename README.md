# $\Large\bf{Beyond~the~View}$

> $\large\sf {
다양한~접근방식~으로
}$   **$\Large\sf {“ 바라볼~수(\sf\bold{View}) ”}$**  $\large\sf {있게~해준~\bold{콘텐츠}}$
> 

<aside>
❗ 여러가지 실감 미디어 콘텐츠 중 **Kinect** 를 활용하여 사용자가 손 쉽게 체험할 수 있는

콘텐츠를 제작하는 것을 목표로 잡고 제작하였습니다. 

해당 콘텐츠를 통하여 실감미디어 콘텐츠가 VR뿐만이 아닌

다양한 접근 방식으로 ***“바라볼 수 ”*** 있다는 것을 깨닫게 해준 콘텐츠입니다.

</aside>

---

# $\Large\bf{Kinect~?}$

<aside>
<img src="/icons/video-camera_gray.svg" alt="/icons/video-camera_gray.svg" width="40px" /> **Kinect** 란? 
**Microsoft** 사에서 직접 개발한 게임기 입니다.
초창기 출시 목적으로는 **게임이 주 목적**이었지만 시간이 지날수록 게임으로서의
역할이 아닌 **체험전시 및 산업 목적**으로 사용이 되기 시작했습니다.

해당 프로젝트는 **Kinect** 기기 중 산업 목적으로 개발된 **Azure Kinect** 를 
사용하여 제작하였습니다.

</aside>

![출처 : https://namu.wiki/w/Kinect#s-1 Kinect 나무위키 문서 ](https://prod-files-secure.s3.us-west-2.amazonaws.com/9f4f04d2-e3d9-4279-8c92-dec2e691c5e0/c83ff8df-bb72-4276-a5a8-b56de5860aba/Untitled.png)

출처 : https://namu.wiki/w/Kinect#s-1 Kinect 나무위키 문서 

# $\Large\bf{실감미디어의~중요성}$

<aside>
<img src="/icons/video-camera_gray.svg" alt="/icons/video-camera_gray.svg" width="40px" />

**실감미디어 제작시 중요성** 

> **“실감미디어 체험 작품을 만들때 중요한것은 체험은 항상 직관,감각적 이여야 한다.”**
> 
> 
> **”사용자가 체험을 하였을때 매우 간단하면서 몰입을 유도해야만 한다.”**
> 
> 위 두 문장은 해당 프로젝트의 중점 목표였습니다.
> 
> 학교 재학당시 실감미디어 수업 중 가르치시던 교수님께서 해주신 말씀입니다.
> 프로젝트를 제작하면서 제일 고민했던 부분이 위 두 문장과 같았습니다.
> 
> **” 어떻게 하면 간단하면서도 직관적인 체험이 가능한 결과물을 제작할수 있을까? ”**
> 라는 생각을 하게 되었고 이 의문점에 대해 깊은 고민을 하게 됐습니다.
> 
> 먼저 **Azure Kinect** 기기의 특징을 살펴 보았습니다.
> **Azure Kinect** 기기는 총 3개의 카메라를 가지고 있습니다.
> 
> <aside>
> <img src="/icons/video-camera_gray.svg" alt="/icons/video-camera_gray.svg" width="40px" /> **1. Depth Camera (깊이 카메라)
> 2. RGB Camera (4K RGB 카메라)
> 3. InFraed Camera (적외선 카메라)**
> 
> </aside>
> 
> 위 3개의 카메라를 사용하여 사용자의 신체 구조와 **Skeleton(뼈 구조)** 를 받아와 
> 신체 정보를 등록하고 컴퓨터 화면에서 보여지게 하는 로직을 가지고 있습니다.
> 
> 그리하여 사용자의 손 구조를 받아와 오브젝트와 태그되면 반으로 갈라지고
> 점수를 얻게 되는 프로젝트를 구상하게 되었습니다.
> 
</aside>

## $\Large\bf{개발~목표}$

<aside>
<img src="/icons/apple_red.svg" alt="/icons/apple_red.svg" width="40px" /> **Azure Kinect 프로젝트 목표**

그리하여 결론은 **“사용자의 흥미 유발과 손쉬운 조작 난이도가 요구되는 콘텐츠”**
라는 결론을 도출하였고 래퍼런스를 찾던 도중 예전에 즐겨했던 모바일 게임인
**”Fruit Ninja”** 게임을 모티브 삼아 프로젝트를 진행하게 되었습니다.

실행 로직은 이러합니다.

> **1.** 사용자는 **Azure Kinect** 앞에 서서 게임을 진행합니다.
> 
> 
> **2.** 사용자의 **손 구조**를 받아와 컴퓨터 화면에서 **손 위치** 에 **오브젝트**를 추가합니다.
> 
> **3.** 사용자가 손을 휘둘러 떨어지는 **과일**에 맞게 되면 **과일이 반으로 쪼개집니다.**
> 
> **4.** 그렇게 쪼개진 과일들이 **믹서기** 안으로 들어가게 되면 **점수**를 얻게 됩니다.
> 
> **5. 더 쪼개진 과일**이 믹서기 안으로 들어가게 되면 **점수**를 더 얻게 됩니다**.**
> 

프로젝트 제작 전 이러한 실행 로직을 기획한 뒤 개발에 진입하게 되었습니다.

그러기 위해서는 **Azrue Kinect** 가 어떠한 방식으로 사용자의 뼈 구조를 받아오는지
알아야 했습니다.

**Azrue Kinect** 를 유니티 에서 사용할 수 있게 해주는 에셋인 **Kinect V2** 의 예제를
사용하여 제작했습니다.

</aside>

## $\Large\bf{개발~과정}$

<aside>
❗ 스크립트를 제작하기에 앞서 너무 나도 방대한 스크립트를 제작하기에는
긴 시간이 필요하였습니다.

그리하여 **Kinect V2** 의 예제를 참고하여 스크립트를 재구성 하였습니다.

</aside>

## $\Large\bf{Scripts}$

### SlashR

```csharp
using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class SlashR : MonoBehaviour
    {
        //플레이어 수를 정하는 변수
        [Tooltip("플레이어의 수")]
        public int playerIndex = 0;
        //KinectInterop.JointType를 통해 사용자의 오른손 관절 값을 받아오는 변수
        [Tooltip("오른손")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;
        //사용자의 손에 부착된 오브젝트 변수
        [Tooltip("사용자의 손에 부착된 오브젝트 변수")]
        public GameObject Slashs;

        //KinectManager의 kinectManager를 받아오는 변수
        private KinectManager kinectManager = null;
        // x 좌표 스케일링 팩터
        public float scaleX = 1f;
        // y 좌표 스케일링 팩터
        public float scaleZ = 1f;
        // 오프셋 벡터
        public Vector3 offset = Vector3.zero;
        public void Start()
        {
            // get reference to KM
            //kinectManager 호출시 KinectManager의 변수 값을 받아온다.
            // Start 시 KinectManager가 활성화가 되고 인스턴스를 참조한다.
            //준비단계
            kinectManager = KinectManager.Instance;
        }
        void Update()
        {

            //키넥트가 준비가 됐고 초기화가 다 되었다면?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //userId = 고유 넘버 아이디 값을 뜻함
                //플레이어 수를 받아옴
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //오른손관절갯수를 받아오는 변수
                int iJointIndex = (int)trackedJoint;
                // 해당 관절이 추적되고 있다면, KinectManager의 GetJointKinectPosition 메소드를 사용하여 
                // 해당 관절의 3D 위치(Unity 좌표계 기준)를 가져온다.
                Vector3 posJoint = kinectManager.GetJointKinectPosition(userId, iJointIndex, true);
                //만약 posJoint 의 위치 정보가 0,0,0 이 아니라면
                if (posJoint != Vector3.zero)
                {
                    //userId 의 값을 통해 iJointIndex 즉 관절 정보를 받아온다.
                    if (kinectManager.IsJointTracked(userId, iJointIndex))
                    {
                        // Vector3 속성의 handPos변수 값을 오른손 관절 값이다.
                        Vector3 handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandRight);
                        //만약 Slashs가 없다면
                        if (Slashs != null)
                        {
                            //handPos.x는 해당 벡터의 x 좌표 값을 받아온다.
                            handPos.x *= scaleX; // scaleX is a scaling factor for the x-coordinate
                                                 //handPos.z는 해당 벡터의 z 좌표 값을 받아온다.
                            handPos.z *= scaleZ; // scaleY is a scaling factor for the y-coordinate
                                                 //handPos의 값은 offset의 값을 더한 값이다.
                            handPos += offset;   // offset is a Vector3 representing the offset in each direction
                                                 //slashPos의 값을 X Z 축만 움직이기 위해 y를 -1f 로 고정하여 값을 받아오고
                            Vector3 slashPos = new Vector3(handPos.x, -1f, handPos.z);
                            //Slashs 오브젝트의 포지션 값은 slashPos값이다.
                            Slashs.transform.position = slashPos;
                        }
                    }
                }
            }
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **SlashR 스크립트 제작중 중점 사항**

> 먼저 **Azrue Kinect** 를 사용하기 위하여 **using com.rfilkov.kinect;** 를 작성하여
> 
> 
> **namespace com.rfilkov.components** 를 통해 **Azrue Kinect** 를 사용할수 있게
> 
> 작성하였습니다.
> 
> 그 뒤에 변수를 작성하였습니다. 
> 
> ```csharp
> **public int playerIndex = 0;** 
> ```
> 
> 해당 변수는 체험할 사용자의 수를 설정해주는 변수입니다. 
> 변수 안 에 숫자를 바꾸면 체험할 수 있는 사용자의 수를 늘릴수 있는 변수입니다.
> 
> 그 다음를 작성하여 Traking 할 변수를 생성했습니다.
> 
> ---
> 
> ```csharp
> public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;
> ```
> 
> 그 다음으로는 **KinectInterop.JointType**를 통해 사용자의 오른손 관절 값을 
> 
> 받아오는 변수를 생성하였습니다. 해당 변수를 통해 체험하는 사용자의 
> 
> 오른손 관절 값을 받아와 카메라를 향해 오른손을 움직이게 되면 화면 상에서
> 
> 오브젝트가 붙은 부분이 따라오게 됩니다.
> 
> ---
> 
> ```csharp
>  public GameObject Slashs;
> ```
> 
> 마지막으로 사용자의 오른손에 붙게될 게임 오브젝트 변수입니다.
> 
> 해당 변수를 통해 사용자가 오른손을 움직이게 되면 오른손에 붙은 오브젝트는 
> 
> 따라오게 됩니다.
> 
> 위 3가지 변수는 사용자에 관련한 변수를 작성하였습니다. 
> 
> ---
> 
> ```csharp
> //KinectManager의 kinectManager를 받아오는 변수
> private KinectManager kinectManager = null;
> // x 좌표 스케일링 팩터
> public float scaleX = 1f;
> // y 좌표 스케일링 팩터
> public float scaleZ = 1f;
> // 오프셋 벡터
> public Vector3 offset = Vector3.zero;
> ```
> 
> 그 다음으로는 **Kinect** 관련 변수들입니다.
> 먼저 **KinectManager kinectManager** 를 통해 **KinectManager** 를 참조할수 있게
> 
> 설정하여 Unity 상에 존재하는 **kinectManager**를 사용하도록 설정하였습니다.
> 
> **scaleX** , **scaleZ** 변수는 오브젝트의 위치를 조정시에 좌표 축별로 스케일을 
> 
> 조정 할 수 있게 설정해주는 변수입니다.
> 
> 마지막으로 **offset** 변수는 오브젝트의 최종 위치를 조정할수 있게 설정해주는
> 
> 변수입니다.
> 
> ---
> 
> **Start()** 부분을 먼저 살펴보겠습니다.
> 
> **Start()** 부분 안을 보면 **kinectManager = KinectManager.Instance;** 이 있습니다.
> 
> 이 함수는 **Unity** 에서 실행을 하였을때 **KinectManager** 가 활성화 됩니다.
> 
> 그 다음 인스턴스를 참조하여 제작한 코드가 실행될수 있게 해주는 함수문입니다.
> 
> ---
> 
> **Update()** 부분을 먼저 살펴보겠습니다.
> 
> 해당 부분에서는 **Unity** 가 실행중일때 계속해서 실행하여하는 코드들이 들어가 
> 
> 있으며 **if 문** 안 조건식을 보시면 **&&** 을 사용하여 **KinectManger** 가 준비 됐고
> 
> **kinectManager.IsInitialized()** 를 사용하여 초기화 됐다면 **if 문** 안의 함수를 
> 
> 실행합니다.
> 
> 그 다음에는 위에서 생성한 **playerIndex** 변수를 통해 player 의 ID 를 받아와
> 
> 몇명의 인원들이 참가하는지 에 대한 값을 받아옵니다.
> 
> **trackedJoint** 변수를 통해 사용자의 오른손 관절값을 받아온 뒤
> 
> **GetJointKinectPosition(userId, iJointIndex, true)** 를 통해 해당 관절에 대한 값을
> 
> **Unity 3D** 좌표 값에 입력하여 화면 상으로 보여지게 만들어줍니다.
> 
> 다음 **if 문** 을 보면 **posJoint**의 값이 Unity 좌표 상에서 0,0,0 이 아니라면 함수를 
> 
> 실행시켜줍니다. 
> 
> 이때 안에 함수를 순서대로 풀어드리면 이러합니다.
> 
> > **1. IsJointTracked** 를 사용하여 사용자의 **ID 값**과 **관절 정보**를 받아옵니다.
> > 
> > 
> > **2. handpos** 라는 변수를 생성하여 오른손을 추적할수 있는 변수를 생성합니다.
> > 
> > **3.** 그 다음 **scaleX**,**scaleZ** 변수를 통해 오른손이 **X**,**Z** 축으로 움직일수 있게 
> >    만들어줍니다.
> > 
> > **4. Y**축은 사용하지 않기 때문**에 offset** 변수를 사용하여 **Y**축은 **-1**로 지정합니다.
> > 
> > **5. slashPos** 라는 **Vector3** 속성의 변수를 생성하여 오른손이 **X**,**Z** 축으로 
> >    이동하였을때 추적하여 따라올수 있게 설정합니다.
> > 
> > **6.** 마지막으로 **Slashs** 라는 **Gameobject** 의 **Position** 값은 **slashPos** 의 값으로
> >    설정하여 **Unity** 상에서 오른손을 따라다니게 제작하였습니다.
> > 
</aside>

### SlashL

```csharp
using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    /// <summary>
    /// JointOverlayer overlays the given body joint with the given virtual object.
    /// </summary>
    public class SlashL : MonoBehaviour
    {
        //플레이어 수를 정하는 변수
        [Tooltip("플레이어의 수")]
        public int playerIndex = 0;
        //KinectInterop.JointType를 통해 사용자의 오른손 관절 값을 받아오는 변수
        [Tooltip("왼손")]
        public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandLeft;
        //사용자의 손에 부착된 오브젝트 변수
        [Tooltip("사용자의 손에 부착된 오브젝트 변수")]
        public GameObject Slashs;

        //KinectManager의 kinectManager를 받아오는 변수
        private KinectManager kinectManager = null;
        // x 좌표 스케일링 팩터
        public float scaleX = 1f;
        // y 좌표 스케일링 팩터
        public float scaleZ = 1f;
        // 오프셋 벡터
        public Vector3 offset = Vector3.zero;
        public void Start()
        {
            // get reference to KM
            //kinectManager 호출시 KinectManager의 변수 값을 받아온다.
            // Start 시 KinectManager가 활성화가 되고 인스턴스를 참조한다.
            //준비단계
            kinectManager = KinectManager.Instance;
        }
        void Update()
        {

            //키넥트가 준비가 됐고 초기화가 다 되었다면?
            if (kinectManager && kinectManager.IsInitialized())
            {
                //userId = 고유 넘버 아이디 값을 뜻함
                //플레이어 수를 받아옴
                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

                //왼손관절갯수를 받아오는 변수
                int iJointIndex = (int)trackedJoint;
                // 해당 관절이 추적되고 있다면, KinectManager의 GetJointKinectPosition 메소드를 사용하여 
                // 해당 관절의 3D 위치(Unity 좌표계 기준)를 가져온다.
                Vector3 posJoint = kinectManager.GetJointKinectPosition(userId, iJointIndex, true);
                //만약 posJoint 의 위치 정보가 0,0,0 이 아니라면
                if (posJoint != Vector3.zero)
                {
                    //userId 의 값을 통해 iJointIndex 즉 관절 정보를 받아온다.
                    if (kinectManager.IsJointTracked(userId, iJointIndex))
                    {
                        // Vector3 속성의 handPos변수 값을 왼손 관절 값이다.
                        Vector3 handPos = kinectManager.GetJointPosition(userId, KinectInterop.JointType.HandLeft);
                        //만약 Slashs가 없다면
                        if (Slashs != null)
                        {
                            //handPos.x는 해당 벡터의 x 좌표 값을 받아온다.
                            handPos.x *= scaleX;
                            //handPos.z는 해당 벡터의 z 좌표 값을 받아온다.
                            handPos.z *= scaleZ;
                            //handPos의 값은 offset의 값을 더한 값이다.
                            handPos += offset;
                            //slashPos의 값을 X Z 축만 움직이기 위해 y를 -1f 로 고정하여 값을 받아오고
                            Vector3 slashPos = new Vector3(handPos.x, -1f, handPos.z);
                            //Slashs 오브젝트의 포지션 값은 slashPos값이다.
                            Slashs.transform.position = slashPos;
                        }
                    }
                }
            }
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **SlashL 스크립트 제작중 중점 사항**

> **SlashL** 스크립트는 **SlashR** 스크립트의 내용과 동일합니다.
> 
> 
> 하지만 변수 부분에서 다른점이 존재합니다.
> 
> ```csharp
> public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandLeft;
> ```
> 
> 바로 **HandRight** 가 아닌 **HandLeft** 점 입니다.
> 
> 이 처럼 **KinectInterop.JointType** 을 사용하고 뒤에 다른 관절을 입력하면 다양한
> 
> 관절을 사용할수 있다는 점이 있습니다.
> 
</aside>

### spawn

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    // 3가지의 오브젝트를 받아오는 배열 변수 지정
    public GameObject[] Fruits;
    // 오브젝트를 생성할 오브젝트 하나 지정
    public GameObject spawnPlane;
    //오브젝트가 생성되는 시간을 설정
    public float spawnInterval = 1f;
    //오브젝트가 생성되는 구역의 가로길이를 지정
    public float planeWidth = 10f;
    //오브젝트가 생성되는 구역의 세로길이를 지정
    public float planeHeight = 10f;

    // 외부에서 값을 받아와 오브젝트 생성을 멈추게 하는 변수
    public bool isSpawning { get; private set; } = true;

    void Start()
    {
        //SpawnFruits 코루틴 시작
        StartCoroutine(SpawnFruits());
    }
    //SpawnFruits 코루틴
    IEnumerator SpawnFruits()
    {
        //isSpawning이 참이면 반복
        while (isSpawning)
        {
            //x축과 z축에 대한 랜덤한 위치를 생성
            float x = Random.Range(-planeWidth / 2, planeWidth / 2);
            float z = Random.Range(-planeHeight / 2, planeHeight / 2);

            //spawnPosition의 값은 spawnPlane의 위치에 x z 값을 포지션값을 받고 y는 0으로 고정
            Vector3 spawnPosition = spawnPlane.transform.position + new Vector3(x, 0f, z);

            // 랜덤으로 하나의 Fruit 선택해서 생성
            int randomIndex = Random.Range(0, Fruits.Length);
            Instantiate(Fruits[randomIndex], spawnPosition, Quaternion.identity);

            //지정된 시간이 경과한 후 다시 실행하게 한다.
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 외부에서 호출하여 스폰을 멈출 수 있는 메소드
    public void StopSpawning()
    {
        isSpawning = false;
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **spawn 스크립트 제작중 중점 사항**

> 해당 스크립트는 하늘에서 떨어지는 과일을 생성하기 위한 스크립트 입니다.
> 
> 
> 변수 부분부터 살펴보시면 **GameObject[]** 를 여러개의 과일을 생성할수 있게 해주는
> 
> 변수입니다.
> 
> **spawnPlane** 이라는 변수는 **Unity** 에서 존재하는 **Palne** 이라는 **GameObject** 이며
> 
> 해당 **GameObject** 의 특징으로는 넓은 판과 같은 성질을 띈다는것입니다.
> 
> 그리하여 Plane 의 성질을 이용하여 **planeWidth** , **planeHeight** 라는 변수를 
> 
> 생성하여 가로 세로의 길이를 지정한뒤 지정한 길이의 크기 면에서 오브젝트가 
> 
> 생성되게 제작하였습니다.
> 
> 그 다음으로는 **spawnInterval** 라는 변수를 생성하여 1초에 한번씩 생성되게 
> 
> 설정하였습니다.
> 
> ---
> 
> ```csharp
> public bool isSpawning { get; private set; } = true;
> ```
> 
> 그 다음으로는 **isSpawning** 변수를 설정하여 외부의 값을 받아와 오브젝트의 생성을
> 
> 멈추게 해주는 변수를 생성하였습니다.
> 
> ---
> 
> 이제 함수를 살펴보겠습니다.
> 
> **Start()** 에서는 **StartCoroutine(SpawnFruits());** 을 사용하여 실행시 코루틴이 
> 
> 실행되게 하였습니다.
> 
> ```csharp
>  IEnumerator SpawnFruits()
>     {
>         //isSpawning이 참이면 반복
>         while (isSpawning)
>         {
>             //x축과 z축에 대한 랜덤한 위치를 생성
>             float x = Random.Range(-planeWidth / 2, planeWidth / 2);
>             float z = Random.Range(-planeHeight / 2, planeHeight / 2);
> 
>             //spawnPosition의 값은 spawnPlane의 위치에 x z 값을 포지션값을 받고 y는 0으로 고정
>             Vector3 spawnPosition = spawnPlane.transform.position + new Vector3(x, 0f, z);
> 
>             // 랜덤으로 하나의 Fruit 선택해서 생성
>             int randomIndex = Random.Range(0, Fruits.Length);
>             Instantiate(Fruits[randomIndex], spawnPosition, Quaternion.identity);
> 
>             //지정된 시간이 경과한 후 다시 실행하게 한다.
>             yield return new WaitForSeconds(spawnInterval);
>         }
>     }
> ```
> 
> **SpawnFruits** 는 과일 오브젝트를 생성해주는 코루틴 함수이며 **While** 문을 
> 
> 사용하여 **isSpawning** 이 참일 경우에 계속해서 반복되게 설정하였습니다.
> 
> 그 다음에는 변수에서 지정한 **planeWidth** , **planeHeight** 길이에 대한
> 
> 랜덤한 생성 위치를 지정하였습니다.
> 
> **spawnPosition** 이라는 변수를 생성하여 **spawnPlane** 의 위치의 **X** , **Z** 의 값을 받고
> 
> **Y** 의 값은 0으로 지정하였습니다.
> 
> 마지막 생성 부분입니다.
> 
> 먼저 **randomIndex** 변수를 이용하여 배열에 저장된 **Gameobject** 중 하나를 
> 
> 랜덤으로 로 선정하여 복제 합니다.
> 
> 그 다음에는 **spawnInterval** 라는 변수와 **WaitForSeconds** 를 사용하여
> 
> 지정한 시간이 지난뒤에 코루틴을 반복하게 하였습니다.
> 
> 마지막으로 외부에서 호출 값을 가져와 생성을 멈출수 있게 해주는
> 
> **isSpawning** 변수를 통해 생성을 멈추게 제작하였습니다.
> 
</aside>

### Fruits

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    //반으로 쪼개진 과일 모델링을 생성하는 변수생성
    public GameObject Fruit1;
    //반으로 쪼개진 과일 모델링을 생성하는 변수생성
    public GameObject Fruit2;
    //private bool isTriggered = false;  
    public GameObject Effects;

    //오디오를 재생 시켜주는 변수 생성 
    public AudioClip audioClip;

    //콜라이더 될때
    private void OnCollisionEnter(Collision other)
    {
        //만약 Slash태그가 붙은 오브젝트와 콜라이더 되면
        if (other.gameObject.CompareTag("Slash"))
        {
            //spawnPosition의 값은 transform.position 이고
            Vector3 spawnPosition = transform.position;
            //AudioPlayer의 인스턴스를 참조하여 오디오를 재생
            AudioPlayer.instance.Play(audioClip);

            //오브젝트를 파괴하고
            Destroy(gameObject);

            //ScoreManager를 찾아 IncreaseScore의 값을 10점 증가
            FindObjectOfType<ScoreManager>().IncreaseScore(10);  // 점수 증가

            //그리고 반으로 쪼개진 오브젝트를 표현하기위해 2개의 오브젝트를 복제
            Instantiate(Fruit1, spawnPosition, Quaternion.identity);
            Instantiate(Fruit2, spawnPosition, Quaternion.identity);
            //후에 쪼개진 이펙트를 넣기 위해 설정
            Instantiate(Effects, spawnPosition, Quaternion.identity);

        }
    }

}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Fruits 스크립트 제작중 중점 사항**

> 해당 스크립트는 과일 오브젝트를 관리하는 오브젝트입니다.
> 
> 
> 과일이 사용자의 손에 붙어있는 Slash 라는 오브젝트와 **Collision** 되면 과일이 반으로
> 
> 쪼개지는 로직을 구현하기 위해 제작하였습니다.
> 
> 먼저 변수 부분입니다.
> 
> 해당 프로젝트에서 생성되는 과일의 종류는 총 3개입니다.
> 
> 과일이 2번 쪼개지는걸 구현하기 위해 **Fruit** , **Fruit1** , **Fruit2** 이라는 변수를 생성한뒤
> 
> 과일이 **Collision** 됐을시 나타나는 **Effets** 와 **Audio** 를 구현하기 위해 **Effects** 라는 변
> 
> 수와 **audioClip** 라는 변수를 생성하였습니다.
> 
> ---
> 
> 다음으로는 **OnCollisionEnter** 함수부분입니다.
> 
> ```csharp
> private void OnCollisionEnter(Collision other)
>     {
>         //만약 Slash태그가 붙은 오브젝트와 콜라이더 되면
>         if (other.gameObject.CompareTag("Slash"))
>         {
>             //spawnPosition의 값은 transform.position 이고
>             Vector3 spawnPosition = transform.position;
>             //AudioPlayer의 인스턴스를 참조하여 오디오를 재생
>             AudioPlayer.instance.Play(audioClip);
> 
>             //오브젝트를 파괴하고
>             Destroy(gameObject);
> 
>             //ScoreManager를 찾아 IncreaseScore의 값을 10점 증가
>             FindObjectOfType<ScoreManager>().IncreaseScore(10);  // 점수 증가
> 
>             //그리고 반으로 쪼개진 오브젝트를 표현하기위해 2개의 오브젝트를 복제
>             Instantiate(Fruit1, spawnPosition, Quaternion.identity);
>             Instantiate(Fruit2, spawnPosition, Quaternion.identity);
>             //후에 쪼개진 이펙트를 넣기 위해 설정
>             Instantiate(Effects, spawnPosition, Quaternion.identity);
> 
>         }
>     }
> ```
> 
> 먼저 해당 스크립트가 적용된 오브젝트가 **Collision** 됐을시에 함수를 실행하게
> 
> 작성하였습니다.
> 
> **If 문**을 사용하여 Slash 라는 태그가 붙은 오브젝트와 **Collision** 된다면
> 
> 쪼개지는 과일 오브젝트는 현재 **Collision** 된 위치 이고 **Audio** 를 재생합니다.
> 
> 그 다음 **Collision** 된 오브젝트를 파괴 한뒤 **FindObjectOfType<ScoreManager>**
> 
> 를 사용하여 **ScoreManager** 를 찾아 10점의 점수를 증가 시켜줍니다.
> 
> 그런 뒤 **Instantiate** 를 사용하여 쪼개진 과일 오브젝트가 나올수 있게 설정했습니다.
> 
</aside>

### AfterImage

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    //ParticleSystem속성의 변수 Particle 지정
    public ParticleSystem Particle;
    // Start is called before the first frame update
    void Start()
    {
        // 이 스크립트가 포함된 게임 오브젝트 또는 그 자식 오브젝트 중에서 
        // ParticleSystem 컴포넌트를 찾아 'Particle' 변수에 할당한다.
        Particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        //만약 Particle이 null 아니라면
        if (Particle! == null)
        {
            //만약 'Particle'이 null이 아니라면, 해당 파티클 시스템의 MainModule을 가져온다.
            ParticleSystem.MainModule main = Particle.main;
            //만약 파티클 시스템의 startRotation 속성이 Constant 모드면
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                //startRotation 값을 현재 게임 오브젝트의 z축 회전 각도(도 단위)를 라디안 단위로 변환한 값으로 설정
                //라디안(radian)은 각의 크기를 측정하는 단위 중 하나 Mathf.Deg2Rad를 사용하여 도 단위 값을 라디안 단위 값으로 변환하기 위해 사용
                main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
            }
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" />  **AfterImage 스크립트 제작중 중점 사항**

> 해당 스크립트는 사용자의 손에 붙어있는 **Slash** 라는 오브젝트의 모습이
> 
> 
> 잔상효과 가 나타날수 있게 제작한 스크립트입니다. 
> 
> 먼저 **ParticleSystem** 를 사용하여 **Particle** 의 속성을 사용할수 있는 변수를 
> 
> 생성합니다. 
> 
> **Start()** 에서는 **GetComponentInChildren** 라는 함수를 사용하여 해당 
> 
> 스크립트가 붙어있는 오브젝트 또는 자식 오브젝트 중 **ParticleSystem** 컴포넌트를 
> 
> 찾아 **'Particle'** 변수에 할당 해줍니다.
> 
> Update() 에서는 **If 문**을 사용하여 **Particle** 이 존재한다면 
> 
> **ParticleSystem.MainModule** 함수를 사용하여 해당 **Particle** 시스템의 **MainModule**
> 
> 을 가져오게 됩니다.
> 만약 **ParticleSystem** 의 **startRotation** 속성이 **Constant** 이라면 **startRotation** 값을
> 
> 현재 게임 오브젝트의 z축 회전 각도를 **radian** 으로 변환한 값으로 설정 하여
> 
> **Mathf.Deg2Rad**를 사용하여 도 단위 값을 **radian** 값으로 변환하기 위해 
> 
> 사용하여 잔상효과를 제작하였습니다.
> 
</aside>

### ScoreManager

```csharp
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

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" />  **ScoreManager 스크립트 제작중 중점 사항**

> 마지막 스크립트로는 **ScoreManager** 스크립트 입니다.
> 
> 
> 눈에 띄는 부분은 바로 **using UnityEngine.UI;** 입니다.
> 
> 해당 **namespace** 를 통해 Unity 안에서 UI 기능을 사용할수 참조 해 줍니다.
> 
> ```csharp
> public int Score { get; private set; } = 0; 
> ```
> 
> 해당 변수는 spawn 스크립트에 있는 변수 와 동일하며 게임 점수를 저장하고 
> 
> **private set** 을 통해 외부에서 수정할수 없게 설정하였습니다. 
> 
> **{ get; private set; }** 에서 **get** 은 프로퍼티의 값을 읽어오며 **set** 은 
> 
> 해당 프로퍼티의 값을 변경할수 있고 **get;** 앞에는 아무런 제한자가 없어 외부에서 
> 
> 값을 받아올수 있지만 **private set;**를 통해 내부에서 값을 수정 못하게 막았습니다.
> 
> ---
> 
> ```csharp
> public Text scoreText;
> ```
> 
> 그 다음으로는 **Unity UI** 시스템을 활용할수 있는 **Text** 속성 변수인 **scoreText**
> 
> 를 생성하여 실행 중에 점수를 볼수 있도록 설정하였습니다.
> 
> 해당 스크립트를 믹서기 오브젝트에 참조시켜 점수를 관리할수 있게 설정하였습니다.
> 
> 그리하여 **Collision** 함수 안의 내용을 살펴보면 태그가 붙은 오브젝트마다 점수를 
> 
> 다르게 설정하여 많이 쪼개진 과일 일수록 점수를 더 얻게 설정하였습니다.
> 
> ---
> 
> ```csharp
> public void IncreaseScore(int increment)
>     {
>         Score += increment;
>         scoreText.text = "Score: " + Score;
>     }
> ```
> 
> 마지막으로는 증가된 점수를 미리 설정한 **Text** 속성의 변수 **scoreText** 에 입력하여
> 
> 점수를 확인할수 있도록 제작하였습니다.
> 
> 해당 함수의 로직은 다음과 같습니다.
> 
> > **1. IncreaseScore** 메소드를 사용하여 매개변수인 **int increment** 를 받습니다.
> > 
> > 
> > **2. Collision** 함수안의 각각 적혀있는 **IncreaseScore()** 함수를 사용하여
> >   **() 안**에 지정한 숫자만큼 **Score** 를 증가 시켜 **scoreText** 변수를 사용하여
> >   화면상에 출력하게됩니다.
> > 
</aside>
