# AI World Admin RPG (Unity 3D Alpha)

Unity C# 기반 3D 알파 프로토타입 구조입니다.

## 구현 기능
- 3D 탑다운/쿼터뷰 플레이어 이동(WASD/방향키) + E 상호작용
- NPC 3종(주민/상점주인/경비병) 데이터 구조
- 행동 로그(ActionLogManager)
- 세계 상태(WorldStateManager)
- Mock AI Admin 판단 + 사건/퀘스트 생성
- Admin 패널 UI 스크립트
- Editor 자동 씬 생성 메뉴

## 빠른 시작 (권장)
1. Unity에서 프로젝트를 엽니다.
2. 메뉴 `Tools > AI World Admin RPG > Build Alpha Scene` 클릭.
3. 자동으로 다음이 생성됩니다.
   - Plane 바닥
   - Player(Capsule, Rigidbody, PlayerController)
   - NPC 3명(Villager, ShopOwner, Guard)
   - Main Camera, Directional Light
   - GameSystems + Manager 스크립트
   - Canvas + Dialogue/Admin UI
4. Play를 누르고 테스트합니다.
   - 이동: WASD/방향키
   - 상호작용: NPC 근처에서 E
   - Admin 패널 버튼: `AI Admin 판단 실행`

## 수동 씬 구성 가이드
1. `GameSystems` 빈 오브젝트 생성
   - `GameManager`, `WorldStateManager`, `ActionLogManager`, `AIAdminManager`, `QuestManager`, `EventManager` 부착
2. Player 오브젝트(3D Capsule/Cube)
   - `Rigidbody`, `Collider`, `PlayerController` 부착
3. NPC 3개(3D Capsule/Cube)
   - 각 오브젝트에 `Collider`, `NPCController`, `NPCInteraction` 부착
   - `NPCInteraction.npcController`에 자기 `NPCController` 연결
   - `AIAdminManager.npcControllers` 리스트에 3 NPC 등록
4. UI Canvas 구성
   - Dialogue Panel + TMP_Text 2개 + 버튼 3개
   - Admin Panel + TMP_Text 5개 + 버튼 1개(실행)
   - `DialogueUI`/`AdminPanelUI` 연결
   - 버튼 OnClick: `DialogueUI.ChooseKind/ChooseIgnore/ChooseHelp`, `AdminPanelUI.OnRunAIAdminClicked`

## 실행 흐름
- 플레이어 이동 후 NPC 근처에서 `E` 입력
- 대화 선택지 선택 → 행동 로그 축적
- Admin 패널에서 "AI Admin 판단 실행" 버튼 클릭
- 세계 상태/사건/퀘스트/NPC 반응 변화 확인

## 미완성/확장 포인트
- LLM API 실제 연동은 `LLMAdminService` 뼈대만 제공
- 세이브/로드 미구현
- 월드 오브젝트 확장(건물/포인트/위험 선택 이벤트) 미구현
