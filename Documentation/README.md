# UtilityPackage

Unity3D 개발 과정에서 불편을 느꼈거나, 필요하다고 느꼈던 기능들을 구현한 All-In-One 패키지

---

## 지원하는 기능 목록
**[디자인 패턴](#Patterns)**

**[부트스트랩](#Bootstrap)**

**[스크립트 템플릿](#ScriptTemplates)**

**[커스텀 어트리뷰트](#CustomAttributes)**

**[컬라이더 이벤트 핸들러](#ColliderEventHandler)**

**[트리거 태스크 스케줄러](#TriggerTaskScheduler)**

---

## Patterns
디자인 패턴 구현을 위해 만들어진 베이스 클래스들, 0.0.8 버전 기준으로는 [싱글턴](#Singleton), [스테이트](#State), [커맨드](#Command), [스트래터지](#Strategy), [더티 플래그](#DirtyFlag) 5종류를 지원.

### Singleton
생성자가 여러 차례 호출되더라도 하나의 객체만을 생성해 리턴하는 유일성 보장 패턴

### State
객체의 상태를 하나의 객체로 캡슐화해서 각 상태에 맞는 행동을 구현하는 디자인 패턴

### Command
요청을 하나의 객체로 캡슐화해서 요청을 보내는 객체와 요청을 처리하는 객체를 분할하는 디자인 패턴

### Strategy
알고리즘을 하나의 객체로 캡슐화해서 알고리즘 사용을 독립화하는 디자인 패턴

### DirtyFlag
객체의 상태 변경을 추적해서 필요할 때에만 연산을 수행하게 만드는 디자인 패턴

---

## Bootstrap
프로그램이 시작될 때 초기화를 보장하는 시스템. Profile에 등록된 Prefab을 생성하고, 설정 파일을 로딩하고, 싱글턴 객체들을 생성

---

## ScriptTemplates
스크립트 파일을 만들 때 기존의 Unity3D ScriptTemplates보다 좀 더 유연하게 템플릿을 추가 / 제거하고, 스크립트 생성 과정에서 키워드 대체를 확장

---

## CustomAttributes
커스텀 에디터를 만들지 않고도 Unity가 기본적으로 지원하지 않는 기능들을 수행할 수 있게 만드는 확장된 Attribute

### PreviewTexture
Custom Editor 없이 Inspector상에서 Texture 미리보기 기능 지원

### Required
String이나 Object Reference 값이 유효하지 않다면 경고 문구를 출력

### ReadOnly
Editor / Runtime 상황 중 원하는 상황에 값 변경을 방지

### ConditionalHide
조건을 충족하지 못한다면 Inspector에 출력하지 않음

---

## ColliderEventHandler

충돌체의 OnCollision / OnTrigger 이벤트 처리를 보조하는 Handler 클래스

---

## TriggerTaskScheduler

여러 개의 동작을 연계해서 하나의 Trigger로 사용할 수 있게 묶어주는 Scheduler 시스템

---