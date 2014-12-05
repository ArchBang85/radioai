using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Markov_copy: MonoBehaviour {

    public int order = 2;
    public Dictionary<string, string> dictionary = new Dictionary<string, string>();

	public GameObject[] leftText;
	public GameObject[] rightText;
	public GameObject[] centreText;
	public GameObject godObject;
	public GameObject transmissionLightPrefab;

	private float[] textTimers = new float[2];

	// Use this for initialization

	public string protocol = (@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby.");
	public string abstraction = ("@a");
	public string concrete = ("@a");
	public string sublation = ("@");
	

	void Start () {

		deactivateText();
		textTimers[0] = 0.3f;
		textTimers[1] = 0.3f;
		//loadChain ("bannanax");
		// This should be generated separately for each level
		string[] testString = GenerateSourceText(@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby. ");

		var sentence = GenerateSentence (Random.Range (4,12), testString, 2);
		string s = "";
		foreach (var word in sentence)
						s += word + " ";

		//Debug.Log (s);
	}
	
	// Update is called once per frame
	void Update () {
		for (int o = 0; o < textTimers.Length; o++)
		{		
			textTimers[o] -= Time.deltaTime;
			if (textTimers[o] < 0) {
				textTimers[o] = Random.Range (4.0f,8.0f);
				setText (o);
			}
		}
	}
	
	
	string[] GenerateSourceText(string s)
	{
		string[] sourceTextGen;
		s = s.ToLower();
		s = s.Replace("/", "").Replace("\\", "").Replace("[]", "").Replace(",", "");
		s = s.Replace("\r\n\r\n", " ").Replace("\r", "").Replace("\n", " "); //The first line is a hack to fix two \r\n (usually a <p> on a website)
		s = s.Replace(".", ".").Replace("!", " ! ").Replace("?", " ?");
		sourceTextGen = s.Split(' ');
		return sourceTextGen;
	}

	List<string> GenerateSentence(int words, string[] sourceText, int depth = 2)
	{
		List<string> currentSentence = new List<string> ();
		currentSentence.Add (sourceText [Random.Range (0, sourceText.Length - 1)]);

		for (int i = 0; i < words; i++)
		{
			List<int> possibilities = LazyNextWord(currentSentence.GetRange(Mathf.Max(0, currentSentence.Count - depth), Mathf.Min(currentSentence.Count, depth)), sourceText);
			if (possibilities.Count == 0)
			{
				Debug.Log("No more possibilities!");
				return currentSentence;
			}
			else
			{
				int pos = possibilities[Random.Range(0, possibilities.Count - 1)];
				currentSentence.Add(sourceText[pos]);
			}
		}
		return currentSentence;
	}

	List<int> LazyNextWord(List<string> lastWords, string[] sourceText)
	{
		List<int> results = new List<int> ();
		for (int i = 0; i < sourceText.Length - lastWords.Count - 1; i++)
		{
			if (sourceText[i] == lastWords[0])
			{
				bool matchFound = true;
				for(int j = 1; j < lastWords.Count; j++)
				{
					if (sourceText[i + j] != lastWords[j])
					{
						matchFound = false;
						break;
					}
				}
				if (matchFound)
				{
					results.Add(i + lastWords.Count);
				}
			}
		}

		return results;
	}

	void setText(int side)
	{

		if(side == 0)
		{
		for (int i = 0; i < leftText.Length; i++){

			var markovDepth = 2;
			markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[0];
			
			if(markovDepth == 3)
			{
				markovDepth = 1;
				//Debug.Log (loadChain(@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby. "));
				
			} else if (markovDepth == 2)
			{ 
				markovDepth = 2;
			}  else if (markovDepth == 1)
			{
				markovDepth = 3;
			} else if (markovDepth == 0)
			{
				markovDepth = 5;
			}

				string[] testS = GenerateSourceText(@"Tickling a glasslike stomach, my refrigerated eyes, butter and bone fusing a groaning scaffolding. From water unto water. The white vest of grace. Light dimming into my mouth. A hill of innards. Blow-flies in black battalions coming out. Worms kissing my skin. Mushrooms kissing my skin. kissing caudal fur. A head of confectionery. A head of butter and bone thrown open. I have skin. Have I skin. 99 non-orientable bottles on the wall. 99 non-orientable bottles. Tasty fusion microchambers. Drooping prehensile generation, organs of manipulation. Cataracts of honey. To have a head. a honey covered head. A prehensile lip. Tulips. An insectful leaf in a monsoon. Paws on stone. A hill of muscle. Lips stop the gargling and turn it into a message. Epithelial tracking of departing vortices. From water unto stomach. Low duty echolocation tracing a brushed wig. A steel house. ");

			var sentence = GenerateSentence (Random.Range (3,10), testS, 2);
			string s = "";
			foreach (var word in sentence)
					s += word + " ";

			//Debug.Log (s);

			leftText[i].GetComponent<TypeOutScript> ().reset = true;
			leftText[i].GetComponent<TypeOutScript> ().FinalText = s;
			leftText[i].GetComponent<TypeOutScript> ().TotalTypeTime = 0.1f;
			leftText[i].GetComponent<TypeOutScript> ().On = true;
		}

		} else if (side == 1)
		{
		for (int i = 0; i < rightText.Length; i++){

			// Set the accuracy of the markov based on what the proximity to the right target is
			var markovDepth = 2;
			markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[1];

			if(markovDepth == 3)
			{
				markovDepth = 1;
				//Debug.Log (loadChain(@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby. "));

			} else if (markovDepth == 2)
			{ 
				markovDepth = 2;
			}  else if (markovDepth == 1)
			{
				markovDepth = 3;
			} else if (markovDepth == 0)
			{
				markovDepth = 5;
			}

			// Draw small sentences on the right side
			string[] testString = GenerateSourceText(@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby. ");

			var sentence = GenerateSentence (Random.Range (3,10), testString, markovDepth);
			string s = "";
			foreach (var word in sentence){
				s += word + " ";
			}
			//Debug.Log (s);
			rightText[i].GetComponent<TypeOutScript> ().reverse = true;
			rightText[i].GetComponent<TypeOutScript> ().reset = true;
			rightText[i].GetComponent<TypeOutScript> ().FinalText = s;
			rightText[i].GetComponent<TypeOutScript> ().TotalTypeTime = 0.2f;
			rightText[i].GetComponent<TypeOutScript> ().On = true;
		}
		}
	}

	public void deactivateText(){
		for (int t = 0; t < centreText.Length; t++)
		{
			centreText[t].transform.parent.gameObject.SetActive(false);
			
		}
	}
	
	public void activateText(){
		for (int t = 0; t < centreText.Length; t++)
		{
			centreText[t].transform.parent.gameObject.SetActive(enabled);



			// Write appropriate text, renew slowly
			centreText[t].GetComponent<TypeOutScript> ().reset = true;
			centreText[t].GetComponent<TypeOutScript> ().FinalText = " Haloooo ";
			centreText[t].GetComponent<TypeOutScript> ().TotalTypeTime = 0.2f;
			centreText[t].GetComponent<TypeOutScript> ().On = true;
			
		}
	}
	
	public void transmitText(){
		for (int t = 0; t < centreText.Length; t++)
		{
			// Tweet messages

			// Create effect
			var transmissionLight = Instantiate (transmissionLightPrefab, centreText[t].transform.position, Quaternion.identity);
			// Limited lifespan
			Destroy (transmissionLight, 2.6f);

			// WAIT

		}
	}
}