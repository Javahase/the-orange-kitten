using System;
using System.Collections.Generic;

namespace Models.LLM
{
    public static class ChatAIModel
    {
        /// <summary>
        /// A translation map of the model enum to the API-internal LLM model names.
        /// </summary>
        private static readonly Dictionary<Model, string> ApiModel = new()
        {
            { Model.Apertus_70B_Instruct_2509, "apertus-70b-instruct-2509" },
            { Model.DeepSeek_R1_0528, "deepseek-r1-0528" },
            { Model.DeepSeek_R1_Distill_Llama_70B, "deepseek-r1-distill-llama-70b" },
            { Model.Gemma_3_27B_Instruct, "gemma-3-27b-it" },
            { Model.GLM_4_7, "glm-4.7" },
            { Model.InternVL_3_5_30B_A3B, "internvl3.5-30b-a3b" },
            { Model.Llama_3_1_SauerkrautLM_70B_Instruct, "llama-3.1-sauerkrautlm-70b-instruct" },
            { Model.MedGemma_27B_Instruct, "medgemma-27b-it" },
            { Model.Llama_3_1_8B_Instruct, "meta-llama-3.1-8b-instruct" },
            { Model.LLama_3_3_70B_Instruct, "llama-3.3-70b-instruct" },
            { Model.Mistral_Large_Instruct, "mistral-large-instruct" },
            { Model.GPT_OSS_120B, "openai-gpt-oss-120b" }
        };

        /// <summary>
        /// Retrieves the corresponding model string from the enum-representation of the model.
        /// </summary>
        /// <param name="model">The enum-representation of the model.</param>
        /// <returns>The API-internal model name.</returns>
        public static string GetApiModel(Model model) => ApiModel[model];
    
        [Serializable]
        public enum Model
        {
            /// <summary>
            /// <p>Organization: Swiss AI (Switzerland)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Apr 2024</p>
            /// <p>Context window in tokens: 65k</p>
            /// <p>Advantages: Fully open-source, Multilingual</p>
            /// <p>Limitations: None</p>
            ///
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Apertus_70B_Instruct_2509,

            /// <summary>
            /// <p>Organization: DeepSeek (Canada)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Dec 2023</p>
            /// <p>Context window in tokens: 32k</p>
            /// <p>Advantages: Great overall performance, reasoning and problem-solving</p>
            /// <p>Limitations: Censorship, political bias</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            DeepSeek_R1_0528,

            /// <summary>
            /// <p>Organization: DeepSeek (Canada)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Dec 2023</p>
            /// <p>Context window in tokens: 32k</p>
            /// <p>Advantages: Great overall performance, faster than R1</p>
            /// <p>Limitations: Censorship, political bias</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            DeepSeek_R1_Distill_Llama_70B,

            /// <summary>
            /// <p>Organization: Google (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Mar 2024</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Gemma_3_27B_Instruct,

            /// <summary>
            /// <p>Organization: Z.ai (Singapore)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Apr 2025</p>
            /// <p>Context window in tokens: 200k</p>
            /// <p>Advantages: Great performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            GLM_4_7,

            /// <summary>
            /// <p>Organization: OpenGVLab (Canada)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Aug 2023</p>
            /// <p>Context window in tokens: 40k</p>
            /// <p>Advantages: Vision, lightweight and fast</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            InternVL_3_5_30B_A3B,

            /// <summary>
            /// <p>Organization: VAGOsolutions x Meta (Germany)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Dec 2023</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: German language skills</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Llama_3_1_SauerkrautLM_70B_Instruct,

            /// <summary>
            /// <p>Organization: Google (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Mar 2024</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Vision, medical knowledge</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            MedGemma_27B_Instruct,

            /// <summary>
            /// <p>Organization: Meta (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Dec 2023</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Fast overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Llama_3_1_8B_Instruct,

            /// <summary>
            /// <p>Organization: Meta (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Dec 2023</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Good overall performance, reasoning and creative writing</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            LLama_3_3_70B_Instruct,

            /// <summary>
            /// <p>Organization: Mistral (France)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Jul 2024</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Good overall performance, coding and multilingual reasoning</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Mistral_Large_Instruct,

            /// <summary>
            /// <p>Organization: OpenAI (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Knowledge cutoff: Jun 2024</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Great overall performance, fast</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            GPT_OSS_120B
        }
    }
}