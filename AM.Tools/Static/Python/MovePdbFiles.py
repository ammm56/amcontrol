from __future__ import annotations

import shutil
from pathlib import Path


def is_release_bin_pdb(path: Path) -> bool:
    parts = [part.lower() for part in path.parts]
    return (
        path.suffix.lower() == ".pdb"
        and "bin" in parts
        and "release" in parts
        and "obj" not in parts
    )


def build_target_name(solution_dir: Path, source_file: Path, target_dir: Path) -> Path:
    target_file = target_dir / source_file.name
    if not target_file.exists():
        return target_file

    relative_parent = source_file.parent.relative_to(solution_dir)
    dir_token = "_".join(relative_parent.parts)
    return target_dir / f"{dir_token}_{source_file.name}"


def main() -> int:
    script_dir = Path(__file__).resolve().parent
    solution_dir = script_dir.parent.parent.parent
    target_dir = script_dir.parent / "Pdb"

    target_dir.mkdir(parents=True, exist_ok=True)

    print(f"[MovePdbFiles] SolutionDir = \"{solution_dir}\"")
    print(f"[MovePdbFiles] PdbDir      = \"{target_dir}\"")

    moved_count = 0
    skipped_count = 0

    for source_file in solution_dir.rglob("*.pdb"):
        source_file = source_file.resolve()

        if target_dir in source_file.parents:
            skipped_count += 1
            continue

        if not is_release_bin_pdb(source_file):
            skipped_count += 1
            continue

        target_file = build_target_name(solution_dir, source_file, target_dir)

        try:
            shutil.move(str(source_file), str(target_file))
            moved_count += 1
            print(f"[MovePdbFiles] moved: \"{source_file}\" -> \"{target_file}\"")
        except Exception as ex:
            print(f"[MovePdbFiles] failed: \"{source_file}\" | {ex}")

    print(f"[MovePdbFiles] moved {moved_count} pdb file(s), skipped {skipped_count} file(s).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())