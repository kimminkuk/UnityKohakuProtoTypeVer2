# UnityKohakuProtoTypeVer2

# 계획 일정 ~4/E
### 개요
  - 시작 시, 랜덤하게 클래스 정해주고 Ground에 직업 선택, 강화 공격, 강화 방어, 강화 스킬 (클래스별로, 3개 준비)
  - 시간 지날수록 강화 직업, 아이템 뿌리고 마지막까지 남아 있으면 승리한다.

### Todo
  - Player
    - 움직임 (Flip x 만 표현)
    - 공격
      - Range ( 10%, 8방향 날아가는것만 확인 )
      - Melee (  0% )
    - 스킬
    - 회피
    - 방어
  - Class
    - Magic
      - 여러 상태 좌표 및 액션 ( 10%, 공격 모션, x,y방향 움직임 )
    - Sword
      - 여러 상태 좌표 및 액션 ( 5%, x,y방향 움직임 )
    - Hammer
      - 여러 상태 좌표 및 액션 ( 5%, x,y방향 움직임 )
    - Axe
    - SwordGuard
    - DarkMagic
  - Enemy
    - Player와 동일하지만, 서로 적대시 한다.
  - TileMap
    - Ground 생성
    - Castle Wall 생성
  - Item
    - 직업
      - 바닥에 뿌려두고 줍고 변경되는거 추가 ( 0% )
    - 무기
    - 방어
    - 스킬
  - UI
    - 시간
    - LEVEL
    - 점수
