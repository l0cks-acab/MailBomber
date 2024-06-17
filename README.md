# MailBomber

A Rust plugin that triggers an MLRS rocket barrage when a player interacts with a mailbox. The plugin also adds configurable notes to the mailbox.

## Author 

Made by herbs.acab

## Features

- Activates an MLRS rocket barrage upon interaction with a mailbox.
- Adds configurable notes to the mailbox.
- Includes commands to set up and clear the MailBomber functionality.
- Permission-based access to commands and interaction.

## Commands

- `/mailbomber` - Activates MailBomber on a mailbox. Adds the notes to the mailbox.
- `/clearmailbomber` - Clears all MailBomber entities.

## Permissions

- `mailbomber.use` - Allows the player to use the `/mailbomber` and `/clearmailbomber` commands and interact with the mailbox.

## Customizing Notes

You can add, remove, or change the messages in the NoteMessages list. Each message will be added as a note to the mailbox.

## Usage

  1. Ensure you have the mailbomber.use permission.
  2. Look at a mailbox and use the /mailbomber command to activate it.
  3. Interact with the mailbox to trigger the MLRS rocket barrage.
  4. Use the /clearmailbomber command to remove all MailBomber entities.

## Installation

1. Download the `MailBomber.cs` file.
2. Place the file in your `oxide/plugins` directory.
3. Reload the plugin with the command `oxide.reload MailBomber`.

## Configuration

The plugin generates a configuration file `MailBomber.json` in the `oxide/config` directory. The configuration allows you to set custom messages for the notes added to the mailbox.

### Default Configuration

```json
{
  "NoteMessages": [
    "Unfortunately due to the crimes you have committed, we will be executing you on the spot.",
    "This is a second configurable message."
  ]
}
