# UtilityPackage - v0.0.9
01 - 04 - 2025

### 변경(Modified)
- 충돌체 이벤트 핸들러 개선

# UtilityPackage - v0.0.9
12 - 03 - 2025
### 추가(Added)
- 글로벌 이벤트 시스템, Event-Driven 구현

# UtilityPackage - v0.0.8
07 - 03 - 2025
### 추가(Added)
- 충돌체 이벤트 핸들러Collider Event Handler
- 절차적 이벤트 처리Sequential Event Scheduler

### 변경(Modified)
ConditionalHide Attribute 개선
TilingWindow 개선

# UtilityPackage - v0.0.7
21 - 02 - 202521
### 추가(Added)
- ScripteTemplates 관리 시스템 추가

### 제거(Removed)
- InputManager 외부로 분리


# UtilityPackage - v0.0.6
10 - 02 - 2025
### 추가(Added)
- Bootstrap 기능

### 변경(Modified)
- Singleton 패턴 클래스의 Persistent 설정 가능

# UtilityPackage - v0.0.5
27 - 01 - 2025
### 추가(Added)
-StringBuilder 등의 최적화 사용을 위한 Static Value 클래스들

# UtilityPackage - v0.0.4
21 - 01 - 2025
### 추가(Added)
-커스텀 패키지 레이아웃 생성을 보조하는 PackageCreater

# UtilityPackage - v0.0.3
17 - 01 - 2025
### 추가(Added)
-GameObject 자체 비활성화

-Texture/Sprite를 Inspector에서 미리 볼 수 있는 PreviewTexture 어트리뷰트
-유효하지 않은 값이 들어가있다면 경고해주는 Required 어트리뷰트
-다른 값과 연동해서 유효한 값이 아니라면 숨겨주는 ConditionalHide 어트리뷰트
-에디터 / 플레이중에만 선택적으로 외부 접근을 막아주는 ReadOnly 어트리뷰트

# UtilityPackage - v0.0.2
08 - 01 - 2025
### 추가(Added)
- 단축키 액션
	- 탭 닫기(Ctrl+W)
	- 인스펙터 잠금(Ctrl+L)
- 인스펙터에서 텍스트 파일 수정

# UtilityPackage - v0.0.1
06 - 01 - 2025
### 추가(Added)
- 기본 Patterns
    - Singleton
    - State
    - Command
    - Strategy
- InputSystem 연동 GlobalInputManager
- 사운드 매니저 클래스
    - BGM
    - SFX
- GameObject 자체 파괴
- UI 확장
    - 버튼(Down / Up 이벤트 분리)
- 데이터 유틸리티
    - INI 파일 관리
    - PlayerPrefs 확장