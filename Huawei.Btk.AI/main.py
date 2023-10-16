import json
import textwrap

import easyocr as ocr

import torch
import transformers
from transformers import AutoTokenizer, AutoModelForCausalLM,pipeline


import json
import textwrap


def llm(prompt,allergies,activeIngredients,defaultLanguage):
        
    B_INST, E_INST = "[INST]", "[/ INST]"
    B_SYS, E_SYS = "<<SYS>>\n", "\n<</SYS>>\n\n"
    DEFAULT_SYSTEM_PROMPT = """The user takes a photograph of the contents of a product and converts the image into text using Optical Character Recognition. I will give you this text, called text, and your goal is to summarise this text and allow the user to identify what they would prefer not to find in the product. """
    SYSTEM_PROMPT = B_SYS + DEFAULT_SYSTEM_PROMPT + E_SYS
    
    access_token = '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'

    tokenizer = AutoTokenizer.from_pretrained("meta-llama/Llama-2-7b-chat-hf",
                                            token=access_token)

    model = AutoModelForCausalLM.from_pretrained("meta-llama/Llama-2-7b-chat-hf",
                                                device_map='auto',
                                                torch_dtype=torch.float16,
                                                token=access_token,
                                                #  load_in_8bit=True,
                                                load_in_4bit=True)
    
    pipe = pipeline("text-generation",
                    model=model,
                    tokenizer= tokenizer,
                    torch_dtype=torch.bfloat16,
                    device_map="auto",
                    do_sample=True,
                    top_k=30,
                    num_return_sequences=1,
                    eos_token_id=tokenizer.eos_token_id
                    )

    def get_prompt(instruction):
        prompt_template =  B_INST + SYSTEM_PROMPT + instruction + E_INST
        return prompt_template

    def cut_off_text(text, prompt):
        cutoff_phrase = prompt
        index = text.find(cutoff_phrase)
        if index != -1:
            return text[:index]
        else:
            return text

    def remove_substring(string, substring):
        return string.replace(substring, "")



    def generate(text):
        prompt = get_prompt(text)
        with torch.autocast('cuda', dtype=torch.float16):
            inputs = tokenizer(prompt, return_tensors="pt").to('cuda')
            outputs = model.generate(**inputs,
                                    max_new_tokens=512,
                                    eos_token_id=tokenizer.eos_token_id,
                                    pad_token_id=tokenizer.eos_token_id,
                                    )
            final_outputs = tokenizer.batch_decode(outputs, skip_special_tokens=True)[0]
            final_outputs = cut_off_text(final_outputs, '</s>')
            final_outputs = remove_substring(final_outputs, prompt)

        return final_outputs#, outputs

    def parse_text(text):
            wrapped_text = textwrap.fill(text, width=100)
            
            return wrapped_text
        
    generated_text = generate(prompt)
    # wrapped_text = parse_text(generated_text)

    return generated_text


def main(jsonfile):
    
    image = jsonfile['image']
    
    age = jsonfile['user']['age']
    tall = jsonfile['user']['tall']
    weight = jsonfile['user']['weight']
    allergies = jsonfile['user']['allergies']
    activeIngredients = jsonfile['user']['activeIngredients']
    defaultLanguage = jsonfile['user']['defaultLanguage']
    
    
# OCR
    image_path = image
    reader = ocr.Reader(['en','tr','fr','ar','de','es','it','pt']) 
    ocrText = reader.readtext(image_path,detail = 0)
    # result = reader.readtext('test_images\\alproYulaf.jpeg',detail = 0)

# llm
    
    prompt = ocrText
    result = llm(prompt,allergies,activeIngredients,defaultLanguage)

    return result

    
