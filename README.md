# Water Balloon Game — Unity Project + GitHub Pages

This repository contains:
- **Unity_ReadyToOpen_Project**: Source project with Editor script to auto-generate the scene and prefabs.
- **WebGL_Build/**: Placeholder HTML for GitHub Pages hosting.

## Running the Game Locally
1. Open this project in Unity 2021.3 LTS or newer.
2. The scene and prefabs will be generated automatically.
3. Press **Play**.

## Hosting on GitHub Pages
1. Push this repo to GitHub.
2. In **Settings → Pages**, set:
   - Branch: `main`
   - Folder: `/WebGL_Build`
3. Your page will be live at:
   ```
   https://<your-username>.github.io/<your-repo>/
   ```

When you have a real Unity WebGL build:
- Build it to the `WebGL_Build` folder, overwriting the placeholder files.
- Push changes to GitHub — Pages will serve the playable build.
