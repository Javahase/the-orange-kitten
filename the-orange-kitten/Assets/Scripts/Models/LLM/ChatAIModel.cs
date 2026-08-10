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
            { Model.DeepSeek_V4_Flash, "deepseek-v4-flash" },
            { Model.Devstral_2_123B_Instruct_2512, "devstral-2-123b-instruct-2512" },
            { Model.Gemma_4_31B_Instruct, "gemma-4-31b-it" },
            { Model.GLM_4_7, "glm-4.7" },
            { Model.Mistral_Medium_3_5_128B, "mistral-medium-3.5-128b" },
            { Model.Llama_3_1_8B_Instruct, "meta-llama-3.1-8b-instruct" },
            { Model.GPT_OSS_120B, "openai-gpt-oss-120b" },
            { Model.Qwen_3_5_122B_A10B, "qwen-3.5-122b-a10b"},
            { Model.Qwen_3_5_397B_A17B, "qwen-3.5-397b-a17b"},
            { Model.Qwen_3_6_27B, "qwen-3.6-27b" },
            { Model.Qwen_3_6_35B_A3B, "qwen-3.6-35b-a3b" },
            { Model.Qwen_3_30B_A3B_Instruct_2507, "qwen3-30b-a3b-instruct-2507" },
            { Model.Qwen_3_Coder_Next, "qwen3-coder-next" },
            { Model.Qwen_3_Omni_30B_A3B_Instruct, "qwen3-omni-30b-a3b-instruct" }
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
            /// <p>Release date: Sep 2025</p>
            /// <p>Context window in tokens: 65k</p>
            /// <p>Advantages: Fully open-source, Multilingual</p>
            /// <p>Limitations: None</p>
            ///
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Apertus_70B_Instruct_2509,
            
            /// <summary>
            /// <p>Organization: DeepSeek (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Apr 2026</p>
            /// <p>Context window in tokens: 1M</p>
            /// <p>Advantages: Great overall performance</p>
            /// <p>Limitations: Censorship</p>
            ///
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            DeepSeek_V4_Flash,
            
            /// <summary>
            /// <p>Organization: Mistral (France)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Dec 2025</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Coding, agentic tasks</p>
            /// <p>Limitations: None</p>
            ///
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Devstral_2_123B_Instruct_2512,
            
            
            /// <summary>
            /// <p>Organization: Google (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Apr 2026</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            ///
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Gemma_4_31B_Instruct,


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
            /// <p>Organization: Mistral (France)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Apr 2026</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Mistral_Medium_3_5_128B,

            /// <summary>
            /// <p>Organization: OpenAI (United States)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Aug 2025</p>
            /// <p>Context window in tokens: 128k</p>
            /// <p>Advantages: Great overall performance, fast</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            GPT_OSS_120B,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Jul 2025</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Good performance, fast</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_30B_A3B_Instruct_2507,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Feb 2026</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Coding</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_Coder_Next,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Sep 2025</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Multimodal</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_Omni_30B_A3B_Instruct,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Feb 2026</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_5_122B_A10B,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Feb 2026</p>
            /// <p>Context window in tokens: 256k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_5_397B_A17B,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Apr 2026</p>
            /// <p>Context window in tokens: 262k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_6_27B,
            
            /// <summary>
            /// <p>Organization: Alibaba Cloud (Peoples Republic of China)</p>
            /// <p>Open: Yes</p>
            /// <p>Release date: Apr 2026</p>
            /// <p>Context window in tokens: 262k</p>
            /// <p>Advantages: Vision, great overall performance</p>
            /// <p>Limitations: None</p>
            /// 
            /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
            /// </summary>
            Qwen_3_6_35B_A3B,
        }
    }
}