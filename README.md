# SelfHostTest
 빌드 테스트
Unity WebGL 프로젝트를 GitHub Pages로 배포하는 테스트 프로젝트입니다.
 
## 🌐 GitHub Pages 배포

이 프로젝트는 GitHub Actions를 사용하여 자동으로 GitHub Pages에 배포됩니다.

### 배포된 사이트 확인

배포된 Unity WebGL 게임은 다음 URL에서 확인할 수 있습니다:
- **URL**: https://gamej2026.github.io/SelfHostTest/

### GitHub Pages 활성화 방법

처음 배포하는 경우, 다음 단계를 따라 GitHub Pages를 활성화해야 합니다:

1. GitHub 저장소로 이동: https://github.com/gamej2026/SelfHostTest
2. **Settings** (설정) 탭 클릭
3. 왼쪽 메뉴에서 **Pages** 클릭
4. **Source** 섹션에서 **GitHub Actions** 선택
5. 저장

### 자동 빌드 및 배포

- `main` 또는 `copilot/deploy-github-pages` 브랜치에 push하면 자동으로 빌드 및 배포됩니다
- **빌드 과정**:
  1. 셀프 호스티드 러너에서 Unity WebGL 빌드 실행
  2. 빌드된 파일의 Gzip 압축 해제 (GitHub Pages 호환성)
  3. 빌드 결과물을 아티팩트로 저장
- **배포 과정**:
  1. 빌드 아티팩트 다운로드
  2. GitHub Pages에 배포
- Actions 탭에서 빌드 및 배포 상태를 확인할 수 있습니다

### 셀프 호스티드 러너 설정

이 프로젝트는 Unity 빌드를 위해 셀프 호스티드 러너를 사용합니다.

**러너 레이블 확인 방법:**
1. 조직 설정 페이지 방문: https://github.com/organizations/gamej2026/settings/actions/runners
2. 러너 목록에서 사용 중인 러너를 클릭
3. 러너의 Labels 섹션 확인 (예: `self-hosted`, `macOS`, `X64` 등)

**워크플로우 설정 조정:**
- 워크플로우 파일 (`.github/workflows/build-and-deploy.yml`)의 `runs-on` 값이 러너의 레이블과 정확히 일치해야 합니다
- 현재 설정: `runs-on: [self-hosted, macOS]`
- 러너 레이블이 다른 경우, 워크플로우 파일을 수정하여 일치시켜야 합니다

**문제 해결:**
- 워크플로우가 "Waiting for a runner to pick up this job..." 상태에서 멈춘 경우:
  1. 러너가 온라인 상태인지 확인
  2. 러너 레이블이 워크플로우의 `runs-on` 값과 일치하는지 확인
  3. 러너가 저장소 또는 조직에 접근 권한이 있는지 확인

## 🎮 프로젝트 정보

- **Unity Version**: 2022.3.59f1
- **Build Target**: WebGL
- **Description**: 셀프 호스팅으로 Action 테스트 되는지 확인

## 📁 프로젝트 구조

```
.
├── Assets/              # Unity 에셋 파일
│   ├── Editor/         # 에디터 스크립트
│   │   └── BuildScript.cs  # WebGL 빌드 스크립트
│   ├── Scripts/        # 게임 로직 스크립트
│   └── Scenes/         # Unity 씬
├── build/              # WebGL 빌드 출력 (GitHub Actions에서 생성)
│   └── webgl/          # Unity WebGL 빌드 결과
│       ├── Build/      # 빌드 파일 (wasm, js, data)
│       ├── TemplateData/   # Unity 템플릿 리소스
│       └── index.html  # 메인 HTML 파일
└── .github/
    └── workflows/
        └── build-and-deploy.yml  # GitHub Actions 워크플로우 (빌드 + 배포)
```

## 🔧 로컬 빌드

Unity Editor에서 빌드하려면:

1. Unity 2022.3.59f1 설치
2. 프로젝트 열기
3. File → Build Settings → WebGL 선택
4. Build 클릭 또는 BuildScript를 사용하여 빌드

## ⚠️ GitHub Pages 배포 시 주의사항

### Gzip 압축 문제 해결

Unity WebGL 빌드가 Gzip 압축을 사용하는 경우, GitHub Pages에서 다음과 같은 에러가 발생할 수 있습니다:

```
Unable to parse Build/build.framework.js.gz! This can happen if build compression was enabled but web server hosting the content was misconfigured to not serve the file with HTTP Response Header "Content-Encoding: gzip" present.
```

**원인**: GitHub Pages는 `.gz` 파일을 제공할 때 `Content-Encoding: gzip` 헤더를 자동으로 설정하지 않아, Unity 로더가 압축된 파일을 올바르게 처리하지 못합니다.

**해결 방법**: 이 저장소의 GitHub Actions 워크플로우는 빌드 후 자동으로 압축 파일을 해제하여 이 문제를 해결합니다. 워크플로우는 다음 작업을 수행합니다:

1. Unity WebGL 빌드 실행
2. 생성된 `.gz` 파일 압축 해제
3. `index.html`에서 `.gz` 확장자 참조 제거
4. GitHub Pages에 배포

**로컬 빌드 시**: 수동으로 빌드한 경우, 다음 명령어로 압축을 해제할 수 있습니다:

```bash
cd build/webgl/Build
gunzip -k build.data.gz
gunzip -k build.framework.js.gz
gunzip -k build.wasm.gz
```

그리고 `build/webgl/index.html`에서 `.gz` 확장자를 제거하여 압축 해제된 파일을 참조하도록 수정해야 합니다.

## 📝 라이센스

이 프로젝트는 테스트 목적으로 만들어졌습니다.
