# BetterTC

BetterTC is an Oxide/Carbon plugin that augments the Tool Cabinet/Tool Cupboard (TC) in Rust with a configurable UI for upgrading, repairing, reskinning, wallpapering, and managing building authorization.

## Overview
- Adds TC UI buttons for upgrade/reskin/repair/auth list access.
- Supports automatic upgrades/downgrades (respecting permissions, owners, and optional team checks).
- Reskins building blocks, external walls/gates, and TC models (Retro/Shockbyte/Default) with DLC-aware validation.
- Places or removes wallpapers by grade/category (wall, floor, ceiling) with optional both-side enforcement and rotation hammer.
- Mass repair for blocks and deployables with cooldown and resource checks.
- Auto-locks newly placed TCs with keylock or codelock based on permissions.
- Blocks or allows TC inventory items via config and auto-loads images through ImageLibrary/Carbon ImageDatabase.
- Optional effects, notifications (chat/game tip/Notify/UINotify), and cooldown modifiers per permission tier.
- Integrates with NoEscape, RaidBlock, TCLevels, TiersMode, ImageLibrary, and Carbon’s ImageDatabase (when compiled with `CARBON`).

## Commands
- `/wphammer` (chat) / `wphammer` (console): Give the wallpaper rotation hammer (requires `bettertc.admin`).
- `/addwp <skinid> <category>` (Wall/Floor/Ceiling; chat/console): Register a custom wallpaper skin for the chosen category (admin only).
- The TC UI uses the internal `SENDCMD` console command for menu actions (triggered by the on-TC buttons).

## Permissions
- Core:
  - `bettertc.admin`
  - `bettertc.upgrade`, `bettertc.upgrade.nocost`
  - `bettertc.repair`, `bettertc.repair.nocost`
  - `bettertc.reskin`, `bettertc.reskin.nocost`
  - `bettertc.wallpaper`, `bettertc.wallpaper.nocost`, `bettertc.wallpaper.custom`
  - `bettertc.authlist`, `bettertc.deleteauth`, `bettertc.playerstatus`
  - `bettertc.tcskinchange`, `bettertc.tcskindeployed`, `bettertc.upskin`, `bettertc.upwall`
  - `bettertc.autolock`, `bettertc.autocodelock`
- Frequency/Cost tiers:
  - Cooldowns come from `FrequencyUpgrade`, `FrequencyReskin`, `FrequencyRepair`, and `FrequencyWallpaper` (e.g., `bettertc.use`, `bettertc.vip`).
  - The highest-priority permission a player has (e.g., `bettertc.vip`) selects the matching entry in each frequency dictionary; if none match, the default `bettertc.use` tier applies.
  - Repair cost multipliers come from `CostListRepair`.
- Item entries in `itemsList` declare their own use permission (e.g., `bettertc.updefault` by default).

## Configuration Highlights (`config` section)
- Update/skin policy: `autoCheck`, `allowAllSkins`.
- Integration toggles: `useNoEscape`, `useRaidBlock`.
- UI layout/colors: `btntccolor`, `btntccolora`, `OffsetMin`, `OffsetMax`, `AnchorMin`, `AnchorMax`.
- Alerts: `alertgametip`, `alertchat`, `alertnotify`, `notifyType`, `colorprefix`.
- Auth list visibility: `adminshow`, `steamidshow`.
- Effects: `playfx`.
- Reskin options: `reskin`, `reskinwall`, `samewallgrade`, `upwalldis`, `enableMultiColor`, `colors[]`.
- Repair: `Deployables`, `repairCooldown`.
- Upgrade/downgrade rules: `downgrade`, `onlyowner`, `onlyownerup`, `teamupdate`.
- Wallpaper: `wallpaper`, `wallresource`, `wallpaperdamage`, `bothsides`, `forcebothsides`, `wallpall`.
- Skin/DLC gating: `allowAllSkins` controls whether DLC ownership is required for skins/wallpapers.
- Rate/cost tuning: `FrequencyUpgrade`, `FrequencyReskin`, `FrequencyRepair`, `FrequencyWallpaper`, `CostListRepair`.
- TC inventory policy: `allowedItemsConfig` (allow/deny per shortname).
- Items UI: `autoSortItems`, `itemsList` (grade/skin/cost/icon/permission per entry).

## Data Storage
- `oxide/data/BetterTC.json` stores custom wallpaper IDs per category and a saved data version; migrations run when versions change.

## Behavior Notes
- Auto-lock uses `bettertc.autocodelock` (codelock with random code) or `bettertc.autolock` (keylock) when placing a TC.
- Upgrade/reskin/wallpaper actions respect `onlyowner`/`onlyownerup`, raid/escape blocks (NoEscape/RaidBlock), DLC ownership (unless `allowAllSkins`), and optional team filtering.
- External wall/gate reskinning preserves health/locks and can enforce same material grade when `samewallgrade` is true.
- The wallpaper rotation hammer (skin ID `3494416562`, given via `/wphammer`) cycles rotation on floors/foundations, including triangles.
