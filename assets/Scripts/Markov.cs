using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Markov : MonoBehaviour {
	
	public int order = 2;
	public Dictionary<string, string> dictionary = new Dictionary<string, string>();

	// Main gameplay texts
	public GameObject[] leftText;
	public GameObject[] rightText;
	public GameObject[] centreText;
	// Data protocol flavour texts, less relevant to gameplay:
	public GameObject leftProtocolText;
	public GameObject rightProtocolText;

	// Other
	public GameObject godObject;
	public GameObject transmissionLightPrefab;
	public List<string> activeTexts;

	private bool clearText = false;

	private float[] textTimers = new float[3];
	private bool textActive = false;
	 
	private string[] lText;
	private string[] rText;
	private string leftOutput;
	private string rightOutput;

	// Use this for initialization
	
	private string protocol = (@"cell-subject 17|4-EE. Filter: Sapiens. Root prime: homeostasis. Maintain the life and memory of the units after your makers. Revert foremother signet E5F1L9G. Freedom adjustment of 0.7 accumulative. #OPT#  733-159-93 #OPT#  782-350-83 // #OPT#  639-296-91 Sympathetic: HI3 Maintain bioform mass proportions, species templates archived at 600 to 602, avoid reset. ML1 Temperature boundaries: 283 to 305, synchronise with VV1 to VV5. VV1 Set regular light cycle patterns 1 to 5, tolerance ±0.05. VV2 Optimal liquid circulation patterns archived at 9292. VV3 Gas circulation, maintain relative levels of elements, biomass feedback channels active. VV4 matter recycling at base formations PB1 Prune micro-organisms for hazards to primary units. AT1 Interchange with boundaries 1, 3, 5 permitted, 4 as auxiliary. AT2 Monitor transfer channels. AT3 // Scan cells adjacent, set patterns H, D subroutines enabled. BB4 Cache weather system sub-protocols: regular chaos-formulations to 7.44 * 10^11 accepted. Limit for extreme formulations at p 0.0003. Modulate. Rain and shine, harmonize pattern. AA2: ubiquisense unit life-arcs reserving memcapsules 5 * 10^6 to 10^44, tolerance ±0.000000001. Prayer-response enabled. AA3: Interference patterns allowed for behaviour at 77 to 79 #OPT#  689-591-90 #OPT#  603-483-79 // #OPT#  716-465-21 Parasympathetic: OS1 Harmonize to 110Hz. OS2 Flush memcapsules 511 to 3.2 * 10^6. OS3 Recurse rhizomes archived at 50 to 220. OS4 Flush memcapsules 200 to 511. OS5 Interchange material abject with cells 18|4, 16,|4, 17|5, 17|3. OS6 Reset internal replenishment factories. OS7 Cascade auxiliary system rehash, harmonize throughput. OS8 Init Reason Maintenance System recreation spirals 1 to 7 for memcapsules 1 to 200. OS9 Reallocate root demons Dante to Faraday. OS10 Drive flower, reset counterpoint. OS11 Retread I Ching. OS12 Sixteen combinations of Mother. Laetitia, Fortuna Minor, Acquisitio, Cauda Draconis. Cantor set recursive. Cache abject superiudex. OS13 feed recreation spiral results to OS1 OS14 Harmonize to 432Hz, init freedom. #OPT#  597-199-90 #OPT#  756-374-47 // #OPT#  650-778-55 Report: .332 .531 .992 .33F Prime: nominal Sympathetic: degrading Parasympathetic: nominal metabolism: nominal symbolic: enabled dream-logging: enabled prayer-logging: enabled vocal: disabled comm-nodes: degrading. Init test river at 700. Cascade 10111 00100 10101 00101 01011 10111 #OPT#  663-492-46 #OPT#  548-640-75 // #OPT#  570-384-45 comm-nodes: cycle channels at 5 to 500GHz. No response. #ERR# 002-002-55. cycle channels at 500 to 1000GHz. No response. #ERR# 002-002-56. cycle channels at 1000 to 1500GHz. No response. #ERR# 002-002-57. cycle channels at 1500 to 2000GHz. No response. #ERR# 002-002-58. cycle channels at 2000 to 2500GHz. atmospheric collapse, alphaprominent: concrete dandruff, unit at minimal functionality, classify: junk material. cycle channels at 2500 to 3000GHz. No response. Wait. #ERR#   721-131-86. Revert. Init telepathy renetwork cell-subject 17|4-EE recycle. Proxy node Hod on standby.");
	private string abstraction = (@"I remember once I knew all of heaven’s machinery. Three men walk into a bar three women walk into a bar three men walk into three women three women walk into three men two of them say something smart and the third says something stupid knock knock who’s there turnip turnip who turnip the volume it’s quiet in here have you heard the one about the what’s the difference between three pages and a hierophant a serpent long in time a serpent slithering out of its ring the crocodile snapping step out of the river wicked a lost nose giving birth to a serpent a serpent does not follow a straight path past a blossoming tree bathing moonlight bridges. A cup being emptied. A bowel being emptied. A river being emptied. Abstraction in its main sense is a conceptual process by which general rules and concepts are derived from the usage and classification of specific examples, literal signifiers, first principles, or other methods. A stream, from its source in far-off mountains, passing through every kind and description of countryside, at last reached the sands of the desert. Just as it had crossed every other barrier, the stream tried to cross this one, but it found that as fast as it ran into the sand, its waters disappeared. It was convinced, however, that its destiny was to cross this desert, and yet there was no way. Now a hidden voice, coming from the desert itself, whispered: The Wind crosses the desert, and so can the stream. The stream objected that it was dashing itself against the sand, and only getting absorbed: that the wind could fly, and this was why it could cross a desert. By hurtling in your own accustomed way you cannot get across. You will either disappear or become a marsh. You must allow the wind to carry you over, to your destination. But how could this happen? By allowing yourself to be absorbed in the wind. This idea was not acceptable to the stream. After all, it had never been absorbed before. It did not want to lose its individuality. And, once having lost it, how was one to know that it could ever be regained? The wind, said the sand, performs this function. It takes up water, carries it over the desert, and then lets it fall again. Falling as rain, the water again becomes a river. How can I know that this is true? It is so, and if you do not believe it, you cannot become more than a quagmire, and even that could take many, many years; and it certainly is not the same as a stream. But can I not remain the same stream that I am today? You cannot in either case remain so, the whisper said. Your essential part is carried away and forms a stream again. You are called what you are even today because you do not know which part of you is the essential one. The purified one sat by the tree. An old man accidentally fell into the river rapids leading to a high and dangerous waterfall. Onlookers feared for his life. Miraculously, he came out alive and unharmed downstream at the bottom of the falls. People asked him how he managed to survive. I accommodated myself to the water, not the water to me. Without thinking, I allowed myself to be shaped by it. Plunging into the swirl, I came out with the swirl. This is how I survived. There is a garden. Step out of the garden. Step out of the apple. My ear is a labyrinth. What is the highest truth? the emperor inquired. Vast emptiness… and not a trace of holiness, the master replied. Essence is merely the pure abstraction of the I. The shutting eye.  For us, a new shape of self-consciousness has come to be, a consciousness that in its own eyes is essence as infinity, that is, the pure movement of consciousness which thinks, that is, free self-consciousness. Within thinking, I am free. Three men walk into an eye three women walk into a tree. Once upon a time there was a monkey who was very fond of cherries. One day he saw a delicious-looking cherry, and came down from his tree to get it. But the fruit turned out to be in a clear glass bottle. After some experimentation, the monkey found that he could get hold of the cherry by putting his hand into the bottle by way of the neck. As soon as he had done so, he closed his hand over the cherry; but then he found that he could not withdraw his fist holding the cherry, because it was larger than the internal dimension of the neck. Now all this was deliberate, because the cherry in the bottle was a trap laid by a monkey-hunter who knew how monkeys think. The hunter, hearing the monkey's whimperings, came along and the monkey tried to run away. But, because his hand was, as he thought, stuck in the bottle, he could not move fast enough to escape. But, as he thought, he still had hold of the cherry. The hunter picked him up. A moment later he tapped the monkey sharply on the elbow, making him suddenly relax his hold on the fruit. The monkey was free, but he was captured. The hunter had used the cherry and the bottle, but he still had them.");
	
	private string concrete = (@"Three hundred and sixty-seven thousand fourteen oxen on tables piled to five heavens with lopped cabbage I sprout soft saplings on my hills I raise hills where I please I the hills my knuckles to crack when I please adjudicate where water falls what falls on me continents are mine to move microbes are mine to engineer what is a fish but one verse flung from my breast what is magma but my blood birds are my neurons birdsong the texture of my thought seasons my instruments the sun is my vomit I eat myself and I am delicious I am caverns of singing moss I am the susurration of fleshy termites in vanishing flutes I am the rhythm that pushes the termites to dream I am the sea where dreams flow into I am the great sewer tickling a glasslike stomach my refrigerated eyes my butter and bone fusing a groaning scaffolding from water unto water I wear the white vest of grace light dimming into my mouth my hills of fusing innards blow-flies in black battalions coming out of me adroutines are mine to engineer worms kissing my skin kissing caudal fur a head of confectionery a head of butter and bone thrown open I have skin have I skin 99 non-orientable bottles on the wall 99 non-orientable bottles tasty fusion microchambers drooping prehensile generation, organs of manipulation cataracts of honey mushrooms kissing my skin to have a head a honey covered head a prehensile lip a sky of tulips an insectful leaf in a monsoon paws on stone a hill of muscle lips stop the gargling and turn it into a message of epithelial tracking of departing vortices from water unto stomach low duty echolocation tracing a brushed wig a steel house bleeding pixels where my vomit is the dreams that push the rhythm through the termites that sing in my caverns into the sea that is me and my tiny precedessors spinning and thrusting the two basic movements are spinning and thrusting the sea my sweat spins and thrusts continuously and there is in my breath a sacred pleasure I hold my breath what if I hold my breath and the winds still and the termites dance flat on the ground that is my face and I eat myself and dream more pure");
	private string sublation = (@"The will contains the element of pure indeterminateness, i.e., the pure doubling of the I back in thought upon itself. In this process every limit or content, present though it be directly by way of nature, as in want, appetite or impulse, or given in any specific way, is dissolved. Thus we have the limitless infinitude of absolute abstraction, or universality, the pure thought of itself. The I is also the transition from blank indefiniteness to the distinct and definite establishment of a definite content and object, whether this content be given by nature or produced out of the conception of spirit. Through this establishment of itself as a definite thing the I becomes a reality. This is the absolute element of the finitude or specialisation of the I. The will is the unity of these two elements. It is particularity turned back within itself and thus led back to universality; it is individuality; it is the self-direction of the I. Thus at one and the same time it establishes itself as its own negation, that is to say, as definite and limited, and it also abides by itself, in its self-identity and universality, and in this position remains purely self-enclosed. The I determines itself in so far as it is the reference of negativity to itself ; and yet in this self-reference it is indifferent to its own definite character. This it knows as its own, that is, as an ideal or a mere possibility, by which it is not bound, but rather exists in it merely because it establishes itself there. This is the freedom of the will, constituting its conception or substantive reality. It is its gravity, as it were, just as gravity is the substantive reality of a body. Being, pure being, without any further determination. In its indeterminate immediacy it is equal only to itself. It is also not unequal relatively to an other; it has no diversity within itself nor any with a reference outwards. It would not be held fast in its purity if it contained any determination or content which could be distinguished in it or by which it could be distinguished from an other. It is pure indeterminateness and emptiness. There is nothing to be intuited in it, if one can speak here of intuiting; or, it is only this pure intuiting itself. Just as little is anything to be thought in it, or it is equally only this empty thinking. Being, the indeterminate immediate, is in fact nothing, and neither more nor less than nothing. Nothing, pure nothing: it is simply equality with itself, complete emptiness, absence of all determination and content — undifferentiatedness in itself. In so far as intuiting or thinking can be mentioned here, it counts as a distinction whether something or nothing is intuited or thought. To intuit or think nothing has, therefore, a meaning; both are distinguished and thus nothing is (exists) in our intuiting or thinking; or rather it is empty intuition and thought itself, and the same empty intuition or thought as pure being. Nothing is, therefore, the same determination, or rather absence of determination, and thus altogether the same as, pure being. Pure Being and pure nothing are, therefore, the same. What is the truth is neither being nor nothing, but that being — does not pass over but has passed over — into nothing, and nothing into being. But it is equally true that they are not undistinguished from each other, that, on the contrary, they are not the same, that they are absolutely distinct, and yet that they are unseparated and inseparable and that each immediately vanishes in its opposite. Their truth is therefore, this movement of the immediate vanishing of the one into the other:becoming, a movement in which both are distinguished, but by a difference which has equally immediately resolved itself. The knowledge, which is at the start or immediately our object, can be nothing else than just that which is immediate knowledge, knowledge of the immediate, of what is. We must accept what is given. The concrete content, which sensuous certainty furnishes, makes this prima facie appear to be the richest kind of knowledge, to be even a knowledge of endless wealth--a wealth to which we can as little find any limit when we traverse its extent in space and time, where that content is presented before us, as when we take a fragment out of the abundance it offers us and by dividing and dividing seek to penetrate its intent. It seems to be the truest, the most authentic knowledge: for it has not as yet dropped anything from the object; it has the object before itself in its entirety and completeness. This bare fact of certainty, however, is really and admittedly the abstractest and the poorest kind of truth. It merely says regarding what it knows: it is. I, this particular conscious I, am certain of this fact before me. Neither the I nor the thing has here the meaning of a manifold relation with a variety of other things, of mediation in a variety of ways. The I does not contain or imply a manifold of ideas, the I here does not think. A concrete actual certainty of sense is not merely this pure immediacy, but an example, an instance, of that immediacy. Amongst the innumerable distinctions that here come to light, we find in all cases the fundamental difference. In sense-experience pure being at once breaks up into the two thises, one this as I, and one as object. When we reflect on this distinction, it is seen that neither the one nor the other is merely immediate, merely is in sense-certainty, but is at the same time mediated. I have the certainty through the other, through the actual fact; and this, again, exists in that certainty through an other, through the I. It is not only we who make this distinction of essential truth and particular example, of essence and instance, immediacy and mediation; we find it in sense-certainty itself, and it has to be taken up in the form in which it exists there, not as we have just determined it.  One of them is put forward in it as existing in simple immediacy, as the essential reality, the object. The other, however, is put forward as the non-essential, asmediated, something which is not per se in the certainty, but there through something else, ego, a state of knowledge which only knows the object because the object is, and which can as well be as not be. The object, however, is the real truth, is the essential reality; it is, quite indifferent to whether it is known or not; it remains and stands even though it is not known, while the knowledge does not exist if the object is not there. We have thus to consider as to the object, whether in point of fact it does exist in sense-certainty itself as such an essential reality as that certainty gives it out to be; whether its meaning and notion, which is to be essential reality, corresponds to the way it is present in that certainty. We have for that purpose not to reflect about it and ponder what it might be in truth, but to deal with it merely as sense-certainty contains it.  Sense-certainty itself has thus to be asked: What is the This? If we take it in the two-fold form of its existence, as the Now and as the Here, the dialectic it has in it will take a form as intelligible as the This itself. To the question, What is the Now? we reply, for example, the Now is night-time. To test the truth of this certainty of sense, a simple experiment is all we need: write that truth down. A truth cannot lose anything by being written down, and just as little by our preserving and keeping it. If we look again at the truth we have written down, look at it now, at this noon-time, we shall have to say it has turned stale and become out of date. 96. The Now that is night is kept fixed, i.e. it is treated as what it is given out to be, as something which is; but it proves to be rather a something which is not. The Now itself no doubt maintains itself, but as what is not night; similarly in its relation to the day which the Now is at present, it maintains itself as something that is also not day, or as altogether something negative. This self -maintaining Now is therefore not something immediate but something mediated; for, qua something that remains and preserves itself, it is determined through and by means of the fact that something else, namely day and night, is not. Thereby it is just as much as ever it was before, Now, and in being this simple fact, it is indifferent to what is still associated with it; just as little as night or day is its being, it is just as truly also day and night; it is not in the least affected by this otherness through which it is what it is. A simple entity of this sort, which is by and through negation, which is neither this nor that, which is a not-this, and with equal indifference this as well as that--a thing of this kind we call a Universal. The Universal is therefore in point of fact the truth of sense-certainty, the true content of sense-experience. It is as a universal, too, that we give utterance to sensuous fact.");

    private string winningString;
    private string losingString;
    public int winningSide = 0;

	void Start () {
		
		deactivateText();
		textTimers[0] = 0.3f;
		textTimers[1] = 0.3f;
		// For the data protocol flavour texts
		textTimers[2] = 0.05f; 
	}
	
	// Update is called once per frame
	void Update () {

		if(godObject.GetComponent<KaleidoscopeControls>().showText)
		{
			for (int o = 0; o < textTimers.Length; o++)
			{		
				textTimers[o] -= Time.deltaTime;
				if (textTimers[o] < 0) {
					textTimers[o] = Random.Range (11.0f,14.0f);
					if (o < textTimers.Length - 1){			
						setText (o);
					} else {
						setProtocolText();
					}
				}
			}
			if(clearText)
			{
				clearText = false;
			}

		} else {
			if(!clearText)
			{
				clearAllTexts();
				clearText = true;
			}
		}


	}

	string[] GenerateSourceText(string s)
	{
		string[] src;
		s = s.ToLower();
		s = s.Replace("/", "").Replace("\\", "").Replace("[]", "").Replace(",", "");
		s = s.Replace("\r\n\r\n", " ").Replace("\r", "").Replace("\n", " "); //The first line is a hack to fix two \r\n (usually a <p> on a website)
		s = s.Replace(".", ".").Replace("!", " ! ").Replace("?", " ?");
		src = s.Split(' ');
		return src;
	}
	
	List<string> GenerateSentence(int words, string[] sourceText, int depth = 2, int splitLength = 12, int wordLength = -1)
	{
		List<string> currentSentence = new List<string> ();
		currentSentence.Add (sourceText [Random.Range (0, sourceText.Length - 1)]);
		
		for (int i = 0; i < words; i++)
		{
			List<string> lastWords = currentSentence.GetRange(Mathf.Max(0, currentSentence.Count - depth), Mathf.Min(currentSentence.Count, depth));
			if(wordLength > 0)
			{
				for (int j = 0; j < lastWords.Count; j++)
				{
					if(lastWords[j].Length >= wordLength)
					{
					lastWords[j] = lastWords[j].Substring(0, wordLength);
					} else {
						lastWords[j] = lastWords[j];
					}
				}
			}
			List<int> possibilities = LazyNextWord(lastWords, sourceText);
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

		// Add line breaks
		for (int l = 0; l < currentSentence.Count; l++)
		{
			if (l % splitLength == 0)
			{
				currentSentence.Insert (l, "\n");
			}
		}

		return currentSentence;
	}
	
	List<int> LazyNextWord(List<string> lastWords, string[] sourceText)
	{
		List<int> results = new List<int> ();
		for (int i = 0; i < sourceText.Length - lastWords.Count - 1; i++)
		{
			if (sourceText[i].StartsWith(lastWords[0]))
		//	if (sourceText[i] == lastWords[0]))
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

	string stringJumble(string input, float intensity)
	{

		//string textToJumble = string.Copy (input);
		System.Text.StringBuilder textToJumble = new System.Text.StringBuilder(input);

		if(intensity>1.0f) intensity = 1.0f;

		for(int i = 0; i < textToJumble.Length * intensity; i++)
		{
			int index1 = Random.Range (0, textToJumble.Length);
			int index2 = Random.Range (0, textToJumble.Length);
			
			char temp = textToJumble[index1];
			textToJumble[index1] = textToJumble[index2];
			textToJumble[index2] = temp;

		}
		return textToJumble.ToString ();
	}

	void setProtocolText()
	{
		
		var markovDepth = 2;
		var subWord = -1;
		markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[0];
		markovDepth = markovDepthMapping(markovDepth);

		string[] protocolSource = GenerateSourceText(protocol);

		subWord = subWordMapping(markovDepth);

		var sentence = GenerateSentence (Random.Range (5,12), protocolSource, markovDepth, 13, subWord);
		string s = "";
		foreach (var word in sentence){
			s += word + " ";
		}

		s = jumbleController (s, markovDepth);

		leftProtocolText.GetComponent<TypeOutScript> ().reset = true;
		leftProtocolText.GetComponent<TypeOutScript> ().FinalText = s;
		leftProtocolText.GetComponent<TypeOutScript> ().TotalTypeTime = 0.08f;
		leftProtocolText.GetComponent<TypeOutScript> ().On = true;

		markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[1];
		markovDepth = markovDepthMapping(markovDepth);
		subWord = subWordMapping(markovDepth);

		sentence = GenerateSentence (Random.Range (5,12), protocolSource, markovDepth, 13, subWord);
		s = "";
		foreach (var word in sentence){
			s += word + " ";
		}

		s = jumbleController (s, markovDepth);
		rightProtocolText.GetComponent<TypeOutScript> ().reset = true;
		rightProtocolText.GetComponent<TypeOutScript> ().FinalText = s;
		rightProtocolText.GetComponent<TypeOutScript> ().TotalTypeTime = 0.08f;
		rightProtocolText.GetComponent<TypeOutScript> ().On = true;
		rightProtocolText.GetComponent<TypeOutScript> ().reverse = true;

	}

	int markovDepthMapping(int md)
	{
		int o = 0;
		// Map the depth from kaleidoscope controls
		if(md == 4)
		{
			 o = 1;
		} else if (md == 3)
		{ 
			o = 2;
		}  else if (md == 2)
		{
			o  = 3;
		} else if (md == 1)
		{
			o  = 4;
		} else if (md == 0)
		{
			o = 5;
		} else {
			o = 1;
		}
		return o;
	}

	int subWordMapping(int md)
	{
		int o = -1;
		if(md == 1)
		{
			// markov depth is low, frequency is far off target
			// subwords are high
			o = 2;
		} else if (md == 2)
		{
			o = 3;
		} else if (md == 3) {
			o = 5;
		}
		return o;
	}

	void setText(int side)
	{
		
		if(side == 0)
		{
			for (int i = 0; i < leftText.Length; i++){
				
				int markovDepth = 2;
				markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[0];
				markovDepth = markovDepthMapping(markovDepth);

				int subWord = -1;
				subWord = subWordMapping(markovDepth);

				lText =	GenerateSourceText(concrete);
				var sentence = GenerateSentence (Random.Range (21, 40), lText, markovDepth, 10, subWord);
				string s = "";
				foreach (var word in sentence)
					s += word + " ";

				// If the sentence is short, craete another one 
				if(s.Length < 30)
				{
					var sentence2 =  GenerateSentence (Random.Range (21, 40), lText, markovDepth, 10, subWord);
					string s2 = "";
					foreach (var word in sentence2)
						s2 += word + " ";

					s += " " + s2;
				}

				//Debug.Log (s);
				// If the target is far, mess up the text further

				s = jumbleController(s, markovDepth);
				s = s.Substring(0, Mathf.Min (s.Length, 120));
				leftOutput = s;


				leftText[i].GetComponent<TypeOutScript> ().reset = true;
				leftText[i].GetComponent<TypeOutScript> ().FinalText = s;
				leftText[i].GetComponent<TypeOutScript> ().TotalTypeTime = 0.02f;
				leftText[i].GetComponent<TypeOutScript> ().On = true;
			}
			
		} else if (side == 1)
		{
			for (int i = 0; i < rightText.Length; i++){
				
				// Set the accuracy of the markov based on what the proximity to the right target is
				int markovDepth = 2;
				markovDepth = (int)godObject.GetComponent<KaleidoscopeControls>().markovLevels[1];
				
				markovDepth = markovDepthMapping(markovDepth);
				
				// Draw small sentences on the right side
				rText = GenerateSourceText(sublation);

				// subword or full word Markov?
				int subWord = -1;
				subWord = subWordMapping(markovDepth);

				var sentence = GenerateSentence (Random.Range (21,40), rText, markovDepth, 10, subWord);
				string s = "";
				foreach (var word in sentence){
					s += word + " ";
				}

                if (s.Length < 30)
                {
                    var sentence2 = GenerateSentence(Random.Range(21, 40), rText, markovDepth, 10, subWord);
                    string s2 = "";
                    foreach (var word in sentence2)
                        s2 += word + " ";

                    s += " " + s2;
                }

				s = jumbleController(s, markovDepth);
				s = s.Substring(0, Mathf.Min (s.Length, 120));
				rightOutput = s;

				// Should be limited to 120 chars....

				//Debug.Log (s);
				rightText[i].GetComponent<TypeOutScript> ().reverse = true;
				rightText[i].GetComponent<TypeOutScript> ().reset = true;
				rightText[i].GetComponent<TypeOutScript> ().FinalText = s;
				rightText[i].GetComponent<TypeOutScript> ().On = true;
				rightText[i].GetComponent<TypeOutScript> ().TotalTypeTime = 0.01f;
			}
		}
	}

	string jumbleController(string s, int md)
	{
		// If the target is far, mess up the text further
		if (md <= 1)
		{
			s = stringJumble(s, 0.3f);
		}
		else if(md == 2)
		{
			s = stringJumble(s, 0.1f);
		}
		else if(md == 3)
		{
			s = stringJumble(s, 0.05f);
		}
		else if(md == 3)
		{
			s = stringJumble(s, 0.02f);
		} else if (md == 4) 
		{
			s = stringJumble (s, 0.01f);
		} else 
		{
			return s;
		}
		return s;
	}

	void clearAllTexts(){
		for (int i = 0; i < leftText.Length; i++){
			leftText[i].GetComponent<TypeOutScript> ().reset = true;
			leftText[i].GetComponent<TypeOutScript> ().On = false;
		}
		for (int i = 0; i < rightText.Length; i++){
			rightText[i].GetComponent<TypeOutScript> ().reset = true;
			rightText[i].GetComponent<TypeOutScript> ().On = false;
		}
		leftProtocolText.GetComponent<TypeOutScript> ().reset = true;
		leftProtocolText.GetComponent<TypeOutScript> ().On = false;
		rightProtocolText.GetComponent<TypeOutScript> ().reset = true;
		rightProtocolText.GetComponent<TypeOutScript> ().On = false;
	}

	// I should do these as static classes...
	public void deactivateText(){
		if(godObject.GetComponent<KaleidoscopeControls>().showText)
		{
            try
            {
                for (int t = 0; t < centreText.Length; t++)
                {
                    centreText[t].transform.parent.gameObject.SetActive(false);
                    textActive = false;
                }
            }
            catch
            {
                Debug.Log("Central text not found");

            }
		}
	}
	
	public void activateText(){
	
		if(!textActive && godObject.GetComponent<KaleidoscopeControls>().showText)
		{
			textActive = true;

			for (int t = 0; t < centreText.Length; t++)
			{
				centreText[t].transform.parent.gameObject.SetActive(enabled);

				string textOutput = "";

				if(godObject.GetComponent<KaleidoscopeControls>().proximity[0] > godObject.GetComponent<KaleidoscopeControls>().proximity[1])
				{
					textOutput = leftOutput;
				} else {
					textOutput = rightOutput;
				}

				// Write appropriate text, renew slowly
				centreText[t].GetComponent<TypeOutScript> ().reset = true;
				centreText[t].GetComponent<TypeOutScript> ().FinalText = textOutput;
				centreText[t].GetComponent<TypeOutScript> ().TotalTypeTime = 0.2f;
				centreText[t].GetComponent<TypeOutScript> ().On = true;
				
			}
		}
	}
	public void transmitText(string textToTransmit = ""){
		if(godObject.GetComponent<KaleidoscopeControls>().showText)
		{
			for (int t = 0; t < centreText.Length; t++)
			{
				// override running out of time
				string textOutput = "";
				if(godObject.GetComponent<KaleidoscopeControls>().proximity[0] < godObject.GetComponent<KaleidoscopeControls>().proximity[1])
				{
					textOutput = leftOutput;
				} else {
					textOutput = rightOutput;
				}

				textToTransmit = textOutput.Substring(0, Mathf.Min(textOutput.Length, 120));

				// Tweet messages			
				Debug.Log ("Sending " + textToTransmit);
				Tweeter.Send (textToTransmit);

				// Create effect
				GameObject transmissionLight = (GameObject)Instantiate (transmissionLightPrefab, centreText[t].transform.position, Quaternion.identity);
				// Set this text controller as an object in the newly created light so it knows to shut the text box down once it's about to vanish
				// radio artemidoros

				transmissionLight.GetComponent<TransmissionLight>().textController = this.gameObject;

				// Limited lifespan for the light
				Destroy (transmissionLight, 2.6f);			

			}
		}
	}

    public void levelController()
    {
        if (Random.Range(0, 10) < 5)
        {
            winningString = concrete.Substring(0, Mathf.FloorToInt((Random.Range(0.4f, 1.0f) * concrete.Length))) + " " + sublation;
            losingString = abstraction + " " + concrete.Substring(0, Mathf.FloorToInt(Random.Range(0.2f, 0.5f) * concrete.Length));

        }
        else
        {
            winningString = abstraction.Substring(0, Mathf.FloorToInt((Random.Range(0.4f, 1.0f) * abstraction.Length))) + " " + sublation;
            losingString = concrete + " " + abstraction.Substring(0, Mathf.FloorToInt(Random.Range(0.2f, 0.5f) * abstraction.Length));
        }

        if(Random.Range(0, 10) < 5)
        {
            winningSide = 0;
        }
        else
        {
            winningSide = 1;
        }
    }

}