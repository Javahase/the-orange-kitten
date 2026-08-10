# The Orange Kitten

_The Orange Kitten_ is a visual novel inspired by the Ace Attorney series, developed for a bachelors thesis. The game features an AI-Chat bot that the player has to interact with to solve uncommon ground problems in a kitchen setting.

## Usage
- Clone this repository
- Open this project using the _Unity 6000.3.6f1_ editor

Here, you can go into three scenes by navigating to the `Scenes`-Directory. The `MainStage`-Scene features the game content, `MainMenu` the entry point where players enter their Participant ID for the study, and `End` contains the end screen.

### Working with the LLM
Currently, this repository uses the [Academic Cloud](https://chat-ai.academiccloud.de/) services for the LLM, limitting the amount of available models to the list of models that the academic cloud provides (for August 2026). The services responsible for the LLMs can be found in the Main Stage: `Services/LLMService`, `Services/InsightService`. You can configure the ChatAI-Service here or create your own. If you want to use ChatAI, request a key from them and create a file `Assets/Resources/Secrets/api_key.txt` in which you put the API Key. This resource won't be shown in git.