# KiepMeta1, 방과 후 학습

## [PM,기획 - 조여준]
 
아래의 내용에 영향이 있을 경우 먼저 상의하고 진행
( https://open.kakao.com/o/gJwbSz4e )

### 기획관련
 수업중에 배운 기술들을 기반으로하고 학생들 스스로 확장하며 실력을 올릴 수 있는 범위.
 
 Web service & Metaverse & Blockchain NFT를 융합하여 서비스할 수 있는 환경을 제작.
- Web service: NFT를 보상으로 하는 Quiz대회 주최 기능. 그에 필요한 Quiz data와 가상World의 여러 구성요소들을 배치하고 꾸밀 수 있게 기능 제공.
- Metaverse: Quiz, World 등의 data를 받아서 사용자에게 제공할 3d 환경을 구축. 그 속에서 사용자들이 실시간 활동을 할 수 있게 제공.
- Blockchain NFT: NFT를 발행, 우승자에게 NFT를 제공, NFT소유자 조회 등을 제공(Web service 환경에 통합되어 제공).
- Etc: 실물을 사용자에게 제공(미정). 
 

#### [참고화면 Minecraft]

![MetaEx1-Minecraft1](https://user-images.githubusercontent.com/104874755/224620237-a16af7ef-30ec-4f96-91fe-f48fc31661a3.png)

 
 
  #### [산출물-NftProject용 Quiz대회]
    - 공통
      온라인 메타버스 Quiz대결 (상금 NFT)
      완료일정: 3.21일
      발표 연습 & 녹화: 3.22일
    - Webservice
      NFT발행 "제x회 Quiz대회 우승 트로피" + 고유 트로피 image지정.
      대회 시작 시간 지정.
      Quiz 문제들 입력.
      NFT id 또는 사용자 이름으로 NFT정보 조회 (NFT이름, 소유자, image 등)
      역대 수상 목록과 정보(NFT이름, NFT id, 수령EOA, 수령자)
    - Metaverse
      NFT정보(이름, 이미지(고정 또는 고유image))
      Quiz대회 정보(시작시간, NFT 등) / Quiz진행
    - NFT
      수업 중 진행...
      
  #### [산출물-UnityProject용 Quiz대회]
    - 공통
      온라인 메타버스 Quiz대결 (상금 NFT)
      완료일정: 4.07금요일
      PPT(각 학생별 본인이 작업한 내역 위주로 정리): 4.10월요일
    - Webservice
      회원 관련 기능(등록, 종류, 정보, 관리)
      Quiz category, 배경 등 추가 정보.
      'Obj배치정보' 관리: 출발지, 물건 등
      NFT Trophy image 선택(약5개중 하나 선택)
    - Metaverse
      회원 인증
      회원 정보(character, EOA, 수상 여부 등)
      Quiz 배경 전환
      대화
      'Obj배치정보' 적용
      NFT Trophy image, model 표시.
    - NFT
      NFT Trophy image id정보(IPFS적용 없이 파일 id만 저장).
      ...
    - 기타
      여러 quiz들
 
### 개발관련
- git에 올리기전 확인 받을 것(임시 공유는 압축파일로 필요한 사람에게 공유).
- 서로 하는 일 알 수 있게 '작업일지' 작성.
- 함수이름: class소속 함수는 소문자로 시작, 일반 또는 static 함수는 대문자로 시작.
- 변수이름: 변수는 소문자로 시작, 상수는 대문자로 시작.
- class이름: class정의는 대문자로 시작.
- file이름: 대문자로 시작(web page관련은 모두 소문자, 구분이 필요할 경우 언더스코어'_'추가).
 
## [개발,구현 - 학생들로 구성]
 (출석부순) 리진, 권창범, 김규환, 반현진, 송두영, 이동규
- Web
- Metaverse
- Blockchain
- Etc

## [작업 목록, 시간, 상황]
전체 작업에서 허점을 줄이려면 서로 생각하는 작업 공유가 중요!.

### 작업 검토, 할당 필요
- NFT image file name (ipfs style)
 연관 blockchain, web, unity
- Quiz data
 문제, 답, 종류, 배경, 난이도
- 캐릭 위 이름표시(2d or 3d)
- 새로운 quiz play 장소 준비
- 지난 우승자 표시
 unity 캐릭 위에, web 계정 정보에서, ..
- Quiz결과에 따른 명확한 표시
 Ex: 관객 반응, particle effect
- 종료 우승자 연출 처리?

### 이동규
- webserver와 db연결 작업 1일, 완료
- web 꾸미기 작업 2일, 완료
- web quiz data db에 채우기 1일, 완료
- web과 server 연결 작업 1일, 완료
- 로그인 화면 구성 3일, 완료
- 캐릭터 색깔 선택 완료, 서버연결 및 유니티 연결 완료
- 회원정보 관리 창 구현 
- 퀴즈 카테고리별 관리 기능 1~2일 미완
- 배경, 맵, Obj 배치 관리 기능 및 unity와 연결 3~4일 미완
- Nft Thohpy 이미지 선택 기능 구현 미완

### 권창범
- 블렌더 모형 캐릭터 3D 모델링 2일
- 모형 캐릭터 뼈(Bone) 연동 3일
- 모형 캐릭터 애니메이션 효과 2일
- Unity 연동 및 기타 수정 및 실행

### 리진
- 서버 연동 (캐릭터 실시간 위치, 상태 등): 1일, 완료.
- Unity 지도 생성: 1일. 완료
- DB에서 상태 저장, 1일, 진행중.
- NFT 발행: 1일, 미완료.
- 게임 UI 디자인, 1일, 미완료.

### 반현진
- 웹 서버 db 연결 1일, 완료
- 웹 quiz데이터 db에 연결 및 저장 1일, 완료
- quiz데이터를 unity에 연결해서 출력 2~3일, 완료
- quiz목록 db 관리 2~3일, 완료
- 대회시작시간 설정 및 유니티 연결 2~3, 완료
- Quiz category 설정 1~2, 진행중
- 퀴즈우승자 데이터 전달 1~2, 미완
- login 및 데이터 저장 2~3, 미완

### 김규환
- 플레이어 이동 및 모션 구현 1일 완료
- 퀴즈 UI 및 db 연결 2일 완료
- 퀴즈 진행 및 구성 2일 완료
- 퀴즈 기능 수정 1일 완료
- 채팅 기능 및 서버 연결 1.5 미완
- 유니티 로그인 기능 1일 완료
- 다른 퀴즈게임 추가 2~3일 미완

### 송두영
- 관중 캐릭터 애니메이션 만들기 2일
- 관중 캐릭터 애니메이터 삽입 1일
- 관중 캐릭터 애니메이션 구현 2일
- 관중 캐릭터 애니메이션 완료
- 관중 캐릭터 애니메이션 퀴즈 게임 접목 미완

# 방과 후 작업 일지


  ## 20230221
    리진: 이날 Main1 의 Update() 함수에서 캐릭터 실시간 위치(x,z)는  NECIAL 서버에 정송 성공했슴.
      그러나 캐릭터 가만히 있을때 도 계속 촤표를 전송하고 있고요. 네트워크 자원 낭비 됐어요.
      그래서 CharacterController에서 캐릭터 움직임을 판단하고, 수정 후 움직일 때만 서버에서 전송 구현 성공. 

  ## 20230222
    리진: 이날 게임 중에 키보드로 UI 화면을 호출하고, Save 버튼을 누르면 현재 이름, 위치 정보를 DB에서 저장 완료.
      이 기능을 통해 게임 다시 로딩할 때, DB로 현재 상태를 복구할 수 있습니다.
   
  ## 20230223
    리진:  지도는 무작위의 일정한 높낮이에 따라 생성하고, 그리고 랜덤하게 보물함을 추가했어요. 

  ## 20230224
    리진:  지도 주면 높은 벽들을 세웠어요. 직접 Mesh를 생성했어요.

  ## 20230303
    김규환 : 유니티 플레이어 점프시 바닥태그("Floor") 설정 필수 (태그 추가 필요시 PlayerMotion코드 - OnCollisionEnter의 조건문에 추가)

  ## 20230314
    금요일 17일, 화요일 21일 완료 내용 계획 필요.
    - 금요일17일 오전까지 완료
      리진: 웹으로 NFT 발행및 조회.
      권창범: Blender 모형 캐릭터 및 애니메이션 효과 적용 ↔ Unity 연동
      김규환: 퀴즈 작동 및 DB 데이터 연결 
      반현진: 퀴즈데이터 연동 및 입,출력 구현
      송두영: 유니티 애니메이터 작업 관리
      이동규: 웹 서비스 대회 시간 지정, 퀴즈 문제들 입력 구현(완료)
      
    - 화요일21일 완료
      리진: NFT웹 페이지 구현.
      권창범: Blender 캐릭터.fbx 추출 ↔ Unity 연동 및 실행   
            
  ![nft_image](https://user-images.githubusercontent.com/86579626/227095263-638a3a44-6aac-42b1-bec5-660f8b589d0e.png)
  
      김규환: 퀴즈 정상출력 관리, 플레이어 데이터 관리
      반현진: 퀴즈목록  데이터 관리
      송두영: 유니티 애니메이터 작업 관리
      이동규: 대회 시작 시간 및 퀴즈 문제 DB에 저장, 역대 수상목록과 정보출력
     - 수요일22일
      발표자료(PPT) 만들기

  ## 20230316
    이동규 : db에서 받아온 text를 유니티에서 출력하는 코드 작성
      유니티에서 실행전 서버 연결 필수
      한글 폰트 설치 필요
![image](https://user-images.githubusercontent.com/104874755/225546756-893c48a6-6777-4551-a68b-774b4cb31b8b.png)

  ## 20230317
    오전 테스트 계획
    
    김규환(커밋 시간 14:30) : 데이터 저장시 (데이터베이스 이름 test, 콜렉션 이름 quiz / String : content, answer 사용) 
    (mongodb Document 저장시 - "content":"문제", "answer":"O" (O, X로 고정)
    
    이동규 : 웹 서비스 템플릿 구성(대회 시간 지정, 퀴즈 문제 입력 구현)
    
  ## 20230322
    김규환 : 플레이어 데이터 저장 (데이터베이스 이름 test, 콜렉션 이름 users / String : name, nftAddr 사용)   
    (예 : name : "홍길동", nftAddr : "0xA5f7D1035cCE55C4642639e06994BD946D3ce718")
    
  ## 20230323
    이동규 : 플레이어 데이터를 받아서 Db에 저장하고 웹에서 출력하는 과정 처리 nft 발행은 미완성
  
  ## 20230324
    리진 :  여거 사용자 네트워크 접속하고 실시간 이동 구현.
        
  ## 20230329
    김규환 : 퀴즈 정상 작동 (오류시 Main오브젝트 Quiz확인)
    리진: 네트워크통신으로 멀티유저 여러 케릭터 동시 이동/제어 왕성. 사용자가 네트워크 과 로컬 게임 전환 가능.
    
  ## 20230330
    이동규 : 회원가입 및 로그인 기능 추가 (퀴즈 변경 및 추가 회원관리는 admin 계정을 만들어서 접속해야 함)
    반현진 : 퀴즈시작은 대회시작시간에 맞춰서 시작
    
  ## 20230403
    김규환 : 유니티 로그인 기능 추가(로그인씬에서 DB데이터에 맞춰 로그인시 플레이어 데이터 )
    이동규 : 캐릭터 색깔 선택 완료 및 UI수정
    리진: 사용자(화원)간 실시간 대화 기능 추가
  
  ## 20230404
    리진: 화원 색깔속성 별로 캑릭터 지정
