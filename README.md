# SelfHostTest

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

### 자동 배포

- `main` 또는 `copilot/deploy-github-pages` 브랜치에 push하면 자동으로 배포됩니다
- GitHub Actions 워크플로우가 `build` 디렉토리의 Unity WebGL 빌드를 GitHub Pages에 배포합니다
- Actions 탭에서 배포 상태를 확인할 수 있습니다

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
│   └── Scenes/         # Unity 씬
├── build/              # WebGL 빌드 출력
│   ├── Build/          # 빌드 파일 (wasm, js, data)
│   ├── TemplateData/   # Unity 템플릿 리소스
│   └── index.html      # 메인 HTML 파일
└── .github/
    └── workflows/
        └── build-and-deploy.yml  # GitHub Actions 워크플로우
```

## 🔧 로컬 빌드

Unity Editor에서 빌드하려면:

1. Unity 2022.3.59f1 설치
2. 프로젝트 열기
3. File → Build Settings → WebGL 선택
4. Build 클릭 또는 BuildScript를 사용하여 빌드

## 📝 라이센스

이 프로젝트는 테스트 목적으로 만들어졌습니다.
